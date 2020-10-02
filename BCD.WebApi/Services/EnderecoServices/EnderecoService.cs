using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BCD.Domain.Entities;
using BCD.Repository.EntitiesRepository.EnderecoRepository;
using BCD.Repository.EntitiesRepository.EnderecosPessoasRepository;
using BCD.WebApi.Dtos;
using BCD.WebApi.Services.Exception;

namespace BCD.WebApi.Services.EnderecoServices
{
    public class EnderecoService
    {
        private IMapper _map { get; }
        private readonly IEnderecoRepository _repo;
        private readonly IEnderecosPessoasRepository _repoEnderecosPessoas;
        public EnderecoService(IMapper map, IEnderecoRepository repo, IEnderecosPessoasRepository repoEnderecosPessoas)
        {
            _map = map;
            _repo = repo;
            _repoEnderecosPessoas = repoEnderecosPessoas;
        }

        // DELETAR
        public async Task<EnderecoDto> Delete(int id)
        {
            var endereco = await _repo.GetByIdAsync(id);

            _repo.Delete(endereco);
            if(await _repo.SaveAsync())
            {
                return _map.Map<EnderecoDto>(endereco);
            }
            throw new ArgumentException("Erro ao persistir no banco!!");
        }
        // ATUALIZAR
        public async Task<EnderecoDto> Update(EnderecoDto enderecoDto)
        {
            var endereco = await _repo.GetByIdAsync(enderecoDto.Id);
            if(endereco == null) throw new NotFoundException("Nenhum endereco encontrado com esse id!");

            _map.Map(endereco, enderecoDto);
            
            _repo.Update(endereco);
            if(await _repo.SaveAsync())
            {
                return enderecoDto;
            }
            throw new ArgumentException("Erro ao persistir dados no banco!");

        }
        // ADICIONAR
        public async Task<EnderecoDto> AddRange(List<EnderecoDto> enderecoDto)
        {
            var ceps = enderecoDto.Select(x => x.CEP).ToList<int>();
            var existeCep = await _repo.ExisteCepCadastrado(ceps);
            
            
            var endereco = _map.Map<Endereco>(enderecoDto);

            if(existeCep.Length > 0)
            {
                foreach(int cep in existeCep)
                {
                    foreach(var item in enderecoDto) 
                    {
                        bool existeEndereco = await _repoEnderecosPessoas.ExisteEndereco(cep,item.IdPessoa);

                        if(existeEndereco)
                        {
                            throw new ArgumentException("Essa pessoa ja tem esse endereco cadastrado!");
                        }
                    }
                }
                
            }
            var enderecos = _map.Map<List<Endereco>>(enderecoDto);
             _repo.AddRange(enderecos);
            
            if(await _repo.SaveAsync())
            {
                List<EnderecosPessoasDto> listEnderecosPessoasDto = new List<EnderecosPessoasDto>();
                enderecos.ForEach(x => {
                        enderecoDto.ForEach(
                            y => {
                                EnderecosPessoasDto enderecosPessoasDto2 = new EnderecosPessoasDto
                                {
                                    DataAtualizacao = DateTime.Now,
                                    EnderecoId = x.Id,
                                    PessoaId = y.IdPessoa
                                };
                                listEnderecosPessoasDto.Add(enderecosPessoasDto2);   
                            }
                        );
                });
                // MAPEAMENTO DE DTO PRA ENTIDADE PARA REALIZAR O INSERT
                var enderecoPessoas = _map.Map<EnderecosPessoas[]>(listEnderecosPessoasDto);

                _repoEnderecosPessoas.AddRange(enderecoPessoas.ToList<EnderecosPessoas>());

                if(await _repoEnderecosPessoas.SaveAsync())
                {
                    return _map.Map<EnderecoDto>(endereco);
                }
            }
            throw new ArgumentException("Erro ao persistir no banco!");
        }
        // ADICIONAR
        public async Task<EnderecoDto> Add(EnderecoDto enderecoDto)
        {
            var existeCep = await _repo.GetByCep(enderecoDto.CEP);
            
            var endereco = _map.Map<Endereco>(enderecoDto);

            if(existeCep != null)
            {
                bool existeEnderecoByIdPessoa = await _repoEnderecosPessoas.ExisteEndereco(existeCep.Id, enderecoDto.IdPessoa);
                if(existeEnderecoByIdPessoa)
                {
                    throw new ArgumentException("Essa pessoa ja tem esse endereco cadastrado!");
                }
                EnderecosPessoasDto enderecosPessoasDto = new EnderecosPessoasDto
                {
                    DataAtualizacao = DateTime.Now,
                    EnderecoId = existeCep.Id,
                    PessoaId = enderecoDto.IdPessoa
                };
                // MAPEAMENTO DE DTO PRA ENTIDADE PARA REALIZAR O INSERT
                var enderecoPessoas = _map.Map<EnderecosPessoas>(enderecosPessoasDto);

                _repoEnderecosPessoas.Add(enderecoPessoas);

                if(await _repoEnderecosPessoas.SaveAsync())
                {
                    return _map.Map<EnderecoDto>(endereco);
                }
            }
            _repo.Add(endereco);
            
            if(await _repo.SaveAsync())
            {
                // PREENCHENDO OS CAMPOS DA TABLE QUE GUARDA O ID DE PESSOA E ENDERECO
                EnderecosPessoasDto enderecosPessoasDto = new EnderecosPessoasDto
                {
                    DataAtualizacao = DateTime.Now,
                    EnderecoId = endereco.Id,
                    PessoaId = enderecoDto.IdPessoa
                };
                // MAPEAMENTO DE DTO PRA ENTIDADE PARA REALIZAR O INSERT
                var enderecoPessoas = _map.Map<EnderecosPessoas>(enderecosPessoasDto);

                _repoEnderecosPessoas.Add(enderecoPessoas);

                if(await _repoEnderecosPessoas.SaveAsync())
                {
                    return _map.Map<EnderecoDto>(endereco);
                }
            }
            throw new ArgumentException("Erro ao persistir no banco!");
        }
        // LISTAR TODOS
        public async Task<EnderecoDto[]> GetAll(){
            var getAll = await _repo.GetAllAsync();

            var getAllDto = _map.Map<EnderecoDto[]>(getAll);

            return getAllDto.ToArray();
        }
        // LISTAR POR ID
        public async Task<EnderecoDto> GetById(int id){
            var getById = await _repo.GetByIdAsync(id);
            if(getById == null) throw new NotFoundException("Nenhum registro encontrado com esse id!");

            var getByIdDto = _map.Map<EnderecoDto>(getById);

            return getByIdDto;
        }
        // LISTAR POR PESQUISA
        public async Task<EnderecoDto[]> GetBySearch(string search) {
            var getBySearch = await _repo.GetBySearchAsync(search);

            var getBySeachDto = _map.Map<EnderecoDto[]>(getBySearch);

            return getBySeachDto.ToArray();
        }
    }
}