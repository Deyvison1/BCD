using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BCD.Domain.Entities;
using BCD.Repository.EntitiesRepository.ContaRepository;
using BCD.WebApi.Dtos;
using BCD.WebApi.Services.Exception;

namespace BCD.WebApi.Services.ContaServices
{
    public class ContaService
    {
        private readonly IContaRepository _repo;

        public IMapper _map { get; }

        public ContaService(IContaRepository repo, IMapper map)
        {
            _repo = repo;
            _map = map;
        }
        // GERAR NUMEROS ALEATORIOS PARA CONTA E AGENCIA
        public async Task<HelperContaDto> GerarContaAndAgencia(int idPessoa)
        {
                var contaByIdPessoa = await _repo.GetByIdPessoaAsync(idPessoa);
                HelperContaDto helperContaDto;
                // SE A PESSOA NAO TIVER CONTA, GERAR AGENCIA E CONTA
                if(contaByIdPessoa == null) 
                {
                    Random random = new Random();
                    int agencia = random.Next(10000, 80000);
                    int conta = random.Next(10000000, 80000000);
                    helperContaDto = new HelperContaDto {
                        Conta = conta,
                        Agencia = agencia
                    };
                    return helperContaDto;
                }
                // CASO A PESSOA TENHA ALGUMA CONTA, APROVEITAR A AGENIA E CONTA
                helperContaDto = new HelperContaDto 
                {
                    Agencia = contaByIdPessoa.DigitosAgencia,
                    Conta = contaByIdPessoa.DigitosConta
                };
                return helperContaDto;
        }
        // ADICIONAR CONTA
        public async Task<ContaDto> Add(ContaDto contaDto)
        {
            var helperConta =  await GerarContaAndAgencia(contaDto.PessoaId);
            contaDto.DigitosConta = helperConta.Conta;
            contaDto.DigitosAgencia = helperConta.Agencia;

            var conta = _map.Map<Conta>(contaDto);

            _repo.Add(conta);

            if(await _repo.SaveAsync()) 
            {
                return _map.Map<ContaDto>(conta);
            }
            throw new ArgumentException("Erro ao persistir dados");
        }
        // ATUALIZAR CONTA
        public async Task<ContaDto> Update(ContaDto contaDto)
        {
            var conta = await _repo.GetByIdAsync(contaDto.Id);
            if(conta == null) throw new NotFoundException("Nenhuma conta encontrada com esse id");

            _map.Map(contaDto, conta);

            _repo.Update(conta);
            if(await _repo.SaveAsync()) 
            {
                return _map.Map<ContaDto>(conta);
            }
            throw new ArgumentException("Erro ao persistir dados");
        }
        // DELETAR CONTA
        public async Task<ContaDto> Delete(int id)
        {
            var conta = await _repo.GetByIdAsync(id);
            if(conta == null) throw new NotFoundException("Nenhuma conta encontrada com esse id");

            _repo.Delete(conta);
            if(await _repo.SaveAsync()) 
            {
                return _map.Map<ContaDto>(conta);
            }
            throw new ArgumentException("Erro ao persistir dados");
        }
        // LISTAR TODAS
        public async Task<ContaDto[]> GetAll()
        {
            var conta = await _repo.GetAllAsync();

            var contaDto = _map.Map<ContaDto[]>(conta);

            return contaDto.ToArray();
        }
        // LISTAR POR ID
        public async Task<ContaDto> GetById(int id)
        {
            var conta = await _repo.GetByIdAsync(id);
            if(conta == null) throw new NotFoundException("Nenhum registro encontrado com esse id");

            var contaDto = _map.Map<ContaDto>(conta);

            return contaDto;
        }
        // LISTAR PESQUISA, AGENCIA E CONTA
        public async Task<ContaDto> GetBySearch(string search)
        {
            var conta = await _repo.GetBySearchAsync(search);
            
            var contaDto = _map.Map<ContaDto>(conta);

            return contaDto;
        }
    }
}