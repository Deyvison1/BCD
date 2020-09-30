using System;
using System.Threading.Tasks;
using AutoMapper;
using BCD.Domain.Entities;
using BCD.Repository.EntitiesRepository.ContaCadastradaRepository;
using BCD.WebApi.Dtos;


namespace BCD.WebApi.Services.ContaCadastradaServices
{
    public class ContaCadastradaService
    {
        private readonly IContaCadastradaRepository _repo;
        private readonly IMapper _map;

        public ContaCadastradaService(IMapper mapper, IContaCadastradaRepository repo)
        {
            _repo = repo;
            _map = mapper;
        }

        public async Task<ContaCadastradaDto> Add(ContaCadastradaDto contaDto)
        {
            var conta = _map.Map<ContaCadastrada>(contaDto);
            
            _repo.Add(conta);
            
            if(await _repo.SaveChangeAsync()) 
            {
                return _map.Map<ContaCadastradaDto>(conta);
            }
            throw new ArgumentException("Erro ao Adicionar!");
        }
        public async Task<bool> ExisteContaCadastrada(int contaId, int pessoaId) 
        {
            return await _repo.ExisteContaCadastrada(contaId, pessoaId);
        }
        public async Task<ContaCadastradaDto[]> GetAll()
        {
            var contas = await _repo.GetAllAsync();

            return _map.Map<ContaCadastradaDto[]>(contas);
        }
        public async Task<ContaCadastradaDto[]> GetAllByPessoaId(int pessoaId)
        {
            var contasByPessoaId = await _repo.GetAllByPessoaIdAsync(pessoaId);

            return _map.Map<ContaCadastradaDto[]>(contasByPessoaId);
        }

    }
}