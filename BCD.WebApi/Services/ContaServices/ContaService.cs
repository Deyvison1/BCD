using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BCD.Domain.Entities;
using BCD.Repository.EntitiesRepository.ContaRepository;
using BCD.Repository.EntitiesRepository.HistoricosContasRepository;
using BCD.WebApi.Dtos;
using BCD.WebApi.Services.Exception;
using BCD.WebApi.Services.HistoricoServices;

namespace BCD.WebApi.Services.ContaServices
{
    public class ContaService
    {
        private readonly IContaRepository _repo;

        public IMapper _map { get; }
        private HistoricoService _historicoServices { get; }
        private readonly IHistoricosContasRepository _repoHistoricosContas;

        public ContaService(IContaRepository repo, IMapper map, HistoricoService historicoServices,
            IHistoricosContasRepository repoHistoricoContas)
        {
            _repoHistoricosContas = repoHistoricoContas;
            _historicoServices = historicoServices;
            _repo = repo;
            _map = map;
        }
        // GERAR NUMEROS ALEATORIOS PARA CONTA E AGENCIA
        public async Task<HelperContaDto> GerarContaAndAgencia(int idPessoa)
        {
            var contaByIdPessoa = await _repo.GetByIdPessoaAsync(idPessoa);
            HelperContaDto helperContaDto;
            // SE A PESSOA NAO TIVER CONTA, GERAR AGENCIA E CONTA
            if (contaByIdPessoa == null)
            {
                Random random = new Random();
                int agencia = random.Next(10000, 80000);
                int conta = random.Next(10000000, 80000000);
                helperContaDto = new HelperContaDto
                {
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
            var helperConta = await GerarContaAndAgencia(contaDto.PessoaId);
            contaDto.DigitosConta = helperConta.Conta;
            contaDto.DigitosAgencia = helperConta.Agencia;
            
            if(contaDto.TipoConta == 0)
            {
                bool existeContaCorrete = await _repo.ExisteContaCorrente(contaDto.PessoaId);
                if(existeContaCorrete)
                {
                    throw new ArgumentException("Ja existe uma conta corrente para essa pessoa");
                }
            }
            else if(contaDto.TipoConta != 0)
            {
                bool existeContaPoupanca = await _repo.ExisteContaPoupanca(contaDto.PessoaId);
                if(existeContaPoupanca)
                {
                    throw new ArgumentException("Ja existe uma conta poupanca para essa pessoa!");
                }

            }
            var conta = _map.Map<Conta>(contaDto);
            
            
            _repo.Add(conta);

            if (await _repo.SaveAsync())
            {
                return _map.Map<ContaDto>(conta);
            }
            throw new ArgumentException("Erro ao persistir dados");
        }
        // ATUALIZAR CONTA
        public async Task<ContaDto> Update(ContaDto contaDto)
        {
            var conta = await _repo.GetByIdAsync(contaDto.Id);
            if (conta == null) throw new NotFoundException("Nenhuma conta encontrada com esse id");

            _map.Map(contaDto, conta);

            _repo.Update(conta);
            if (await _repo.SaveAsync())
            {
                return _map.Map<ContaDto>(conta);
            }
            throw new ArgumentException("Erro ao persistir dados");
        }
        // DELETAR CONTA
        public async Task<ContaDto> Delete(int id)
        {
            var conta = await _repo.GetByIdAsync(id);
            if (conta == null) throw new NotFoundException("Nenhuma conta encontrada com esse id");

            _repo.Delete(conta);
            if (await _repo.SaveAsync())
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
            if (conta == null) throw new NotFoundException("Nenhum registro encontrado com esse id");

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
        // REALIZAR SAQUE
        public async Task<ContaDto> Saque(HelperContaDto contaSaque)
        {
            // OBTER CONTAS DA PESSOA
            var conta = await _repo.ListGetByIdPessoaAsync(contaSaque.PessoaId);
            // OBTER CONTA CORRENTE
            var contaCorrente = conta.FirstOrDefault(
                x => x.TipoConta == 0
            );
            // SE NAO TIVER CONTA CORRENTE LANCO UMA EXCECAO
            if (contaCorrente == null)
            {
                throw new NotFoundException("Nenhumca conta corrente encontrada. Somente e possivel realizar saque de conta corrente!");
            }
            contaCorrente.Saldo -= contaSaque.Quantia;
            _repo.Update(contaCorrente);
            if (await _repo.SaveAsync())
            {
                // INSERT NO HISTORICO
                HistoricoDto historicoDto = new HistoricoDto
                {
                    DescricaoTransacao = "SAQUE CONTA CORRENTE",
                    DigitosConta = contaCorrente.DigitosConta,
                    DigitosAgencia = contaCorrente.DigitosAgencia,
                    TipoConta = "Conta Corrente",
                    Valor = contaSaque.Quantia,
                    DataTransacao = DateTime.Now
                };
                // SERVICE ADD DO HISTORICO
                var historicoAdd = await _historicoServices.Add(historicoDto);

                // INSERT EM HISTORICOCONTAS
                HistoricosContasDto historicosContasDto = new HistoricosContasDto
                {
                    ContaId = contaCorrente.Id,
                    HistoricoId = historicoAdd.Id
                };
                var addHistoricosContas = _map.Map<HistoricosContas>(historicosContasDto);
                _repoHistoricosContas.Add(addHistoricosContas);
                if(await _repoHistoricosContas.SaveAsync())
                {
                    return _map.Map<ContaDto>(contaCorrente);
                }
                return _map.Map<ContaDto>(contaCorrente);
            }
            // SE SAIR DE TODOS IF E PQ DEU ERRO AO PERSISTIR DADOS
            throw new ArgumentException("Erro ao persitir dados no banco!");
        }
    }
}