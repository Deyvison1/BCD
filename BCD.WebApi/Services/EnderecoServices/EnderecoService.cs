using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BCD.Domain.Entities;
using BCD.Repository.EntitiesRepository.EnderecoRepository;
using BCD.WebApi.Dtos;
using BCD.WebApi.Services.Exception;

namespace BCD.WebApi.Services.EnderecoServices
{
    public class EnderecoService
    {
        private IMapper _map { get; }
        private readonly IEnderecoRepository _repo;
        public EnderecoService(IMapper map, IEnderecoRepository repo)
        {
            _map = map;
            _repo = repo;
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

            _map.Map(enderecoDto,endereco);
            
            _repo.Update(endereco);
            if(await _repo.SaveAsync())
            {
                return _map.Map<EnderecoDto>(endereco);
            }
            throw new ArgumentException("Erro ao persistir dados no banco!");

        }

        // ADICIONAR
        public async Task<EnderecoDto[]> AddRange(List<EnderecoDto> enderecoDto)
        {   
            var endereco = _map.Map<List<Endereco>>(enderecoDto);

            _repo.AddRange(endereco);

            if(await _repo.SaveAsync())
            {
                return _map.Map<EnderecoDto[]>(endereco);
            }
            
            throw new ArgumentException("Erro ao persistir no banco!");
        }
        // ADICIONAR
        public async Task<EnderecoDto> Add(EnderecoDto enderecoDto)
        {
            var existeCep = await _repo.GetByCep(enderecoDto.CEP);
            
            var endereco = _map.Map<Endereco>(enderecoDto);

            _repo.Add(endereco);
            
            if(await _repo.SaveAsync())
            {
                return _map.Map<EnderecoDto>(endereco);
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