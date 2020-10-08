using System;
using System.Threading.Tasks;
using AutoMapper;
using BCD.Domain.Entities;
using BCD.Repository.EntitiesRepository.PessoaEnderecoRepository;
using BCD.WebApi.Dtos;

namespace BCD.WebApi.Services.PessoasEnderecosServices
{
    public class PessoasEnderecosService
    {
        public IMapper _map { get; }
        public IPessoasEnderecosRepository _repo { get; }
        public PessoasEnderecosService(IMapper map, IPessoasEnderecosRepository repo)
        {
            _repo = repo;
            _map = map;
        }
        // ADICIONAR LISTA
        public async Task<PessoasEnderecosDto[]> AddRange<T>(T[] entidadesDto)
        {
            var entidade = _map.Map<PessoasEnderecos[]>(entidadesDto);

            _repo.AddRange(entidade);
            if(await _repo.SaveChangeAsync()) 
            {
                return _map.Map<PessoasEnderecosDto[]>(entidade);
            }
            throw new ArgumentException("Erro ao persistir dados no banco!");
        }
        // ADICIONAR UM
        public async Task<PessoasEnderecosDto> Add<T>(T entidadeDto)
        {
            var entidade = _map.Map<PessoasEnderecos>(entidadeDto);

            _repo.Add(entidade);
            if(await _repo.SaveChangeAsync()) 
            {
                return _map.Map<PessoasEnderecosDto>(entidade);
            }
            throw new ArgumentException("Erro ao persistir dados no banco!");
        }
    }
}