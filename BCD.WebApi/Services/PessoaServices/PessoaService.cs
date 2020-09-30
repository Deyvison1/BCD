using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BCD.Domain.Entities;
using BCD.Repository.EntitiesRepository.PessoaRepository;
using BCD.WebApi.Dtos;
using BCD.WebApi.Services.EnderecoServices;
using BCD.WebApi.Services.Exception;

namespace BCD.WebApi.Services.PessoaServices
{
    public class PessoaService
    {
        private readonly IPessoaRepository _repo;
        private readonly IMapper _map;
        private readonly EnderecoService _enderecoService;

        public PessoaService(IPessoaRepository repo, IMapper map, EnderecoService enderecoService)
        {
            _repo = repo;
            _map = map;
            _enderecoService = enderecoService; 
        }
        public async Task<bool> ExisteCPF(string cpf){
            bool existe = await _repo.ExisteCPF(cpf);
            if(existe) {
                throw new ArgumentException("Ja existe uma pessoa com esse cpf cadastrado!");
            }
            return existe;
        }
        // ATUALIZAR
        public async Task<PessoaDto> Update(PessoaDto pessoaDto)
        {
            var pessoa = await _repo.GetByIdAsync(pessoaDto.Id);
            if(pessoa == null) throw new NotFoundException("Nenhuma pessoa encontrada com esse id");

            _map.Map(pessoaDto, pessoa);

            _repo.Update(pessoa);
            if(await _repo.SaveAsync()) {
                return _map.Map<PessoaDto>(pessoa);
            }
            throw new ArgumentException("Erro ao persistir dados!");
        }
        // ADICIONAR
        public async Task<PessoaDto> Add(PessoaDto pessoaDto)
        {
            await ExisteCPF(pessoaDto.CPF);
            var pessoa = _map.Map<Pessoa>(pessoaDto);

            _repo.Add(pessoa);
            if(await _repo.SaveAsync()) {
                return _map.Map<PessoaDto>(pessoa);
            }
            throw new ArgumentException("Erro ao persistir dados!");
        }
        // DELETAR
        public async Task<PessoaDto> Delete(int id)
        {
            var pessoa = await _repo.GetByIdAsync(id);
            if(pessoa == null) throw new NotFoundException("Nenhuma pessoa encontrada com esse id");

            _repo.Delete(pessoa);
            if(await _repo.SaveAsync()) {
                return _map.Map<PessoaDto>(pessoa);
            }
            throw new ArgumentException("Erro ao persistir dados!");
        }
        // LISTAR TODOS
        public async Task<PessoaDto[]> GetAll()
        {
            var pessoa = await _repo.GetAllAsync();

            var pessoaDto = _map.Map<PessoaDto[]>(pessoa);

            return pessoaDto.ToArray();
        }
        // LISTAR CPF PELO ID
        public async Task<string> GetCpfById(int idPessoa)
        {
            string cpf = await _repo.GetCpfByIdPessoa(idPessoa);

            return cpf;
        }

        // LISTAR POR ID PESSOA
        public async Task<PessoaDto[]> GetAllByIdPessoa(int idPessoa)
        {
            var pessoa = await _repo.GetAllPessoaById(idPessoa);
            var pessoaDto = _map.Map<PessoaDto[]>(pessoa);

            return pessoaDto;
        }
        // LISTAR POR ID
        public async Task<PessoaDto> GetById(int id)
        {
            var pessoa = await _repo.GetByIdAsync(id);
            if(pessoa == null) throw new NotFoundException("Nenhuma pessoa encontda com esse id!");
            var pessoaDto = _map.Map<PessoaDto>(pessoa);

            return pessoaDto;
        }
        // LISTAR POR PESQUISA, NOME OU CPF
        public async Task<PessoaDto> GetBySearch(string search)
        {
            var pessoa = await _repo.GetBySearchAsync(search);
            
            var pessoaDto = _map.Map<PessoaDto>(pessoa);

            return pessoaDto;
        }
    }
}