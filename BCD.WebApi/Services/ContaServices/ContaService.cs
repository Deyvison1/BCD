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
            var conta = await _repo.GetByAgenciaAndContaCorrente(contaSaque.Agencia, contaSaque.Conta);
            if (conta == null)
            {
                throw new NotFoundException("Nenhumca conta corrente encontrada. Somente e possivel realizar saque de conta corrente!");
            }
            conta.Saldo -= contaSaque.Quantia;

            _repo.Update(conta);
            if (await _repo.SaveAsync())
            {
                // INSERT NO HISTORICO
                HistoricoDto historicoDto = new HistoricoDto
                {
                    DescricaoTransacao = "SAQUE CONTA CORRENTE",
                    DigitosConta = conta.DigitosConta,
                    DigitosAgencia = conta.DigitosAgencia,
                    TipoConta = "Conta Corrente",
                    Valor = contaSaque.Quantia,
                    DataTransacao = DateTime.Now,
                    NomeConta = conta.NomeConta
                };
                // SERVICE ADD DO HISTORICO
                var historicoAdd = await _historicoServices.Add(historicoDto);

                // INSERT EM HISTORICOCONTAS
                HistoricosContasDto historicosContasDto = new HistoricosContasDto
                {
                    ContaId = conta.Id,
                    HistoricoId = historicoAdd.Id,
                    DataCriacao = DateTime.Now
                };
                var addHistoricosContas = _map.Map<HistoricosContas>(historicosContasDto);
                _repoHistoricosContas.Add(addHistoricosContas);
                if(await _repoHistoricosContas.SaveAsync())
                {
                    return _map.Map<ContaDto>(conta);
                }
            }
            // SE SAIR DE TODOS IF E PQ DEU ERRO AO PERSISTIR DADOS
            throw new ArgumentException("Erro ao persitir dados no banco!");
        }
        // REALIZAR DEPOSITO
        public async Task<ContaDto> Depositar(HelperContaDto contaDto)
        {
            // OBTER A CONTA CORRENTE
            var conta = await _repo.GetByAgenciaAndContaCorrente(contaDto.Agencia, contaDto.Conta);
            if (conta == null) throw new NotFoundException("Nehuma conta encontrada!!");

            conta.Saldo += contaDto.Quantia;

            _repo.Update(conta);
            if(await _repo.SaveAsync())
            {
                HistoricoDto historicoDto = new HistoricoDto
                {
                    DataTransacao = DateTime.Now,
                    TipoConta = "Conta Corrente",
                    DescricaoTransacao = "DEPOSITO EM CONTA CONRRENTE",
                    DigitosAgencia = conta.DigitosAgencia,
                    DigitosConta = conta.DigitosConta,
                    Valor = contaDto.Quantia,
                    NomeConta = conta.NomeConta
                };

                var addHistorico = await _historicoServices.Add(historicoDto);

                HistoricosContasDto historicosContasDto = new HistoricosContasDto
                {
                    ContaId = conta.Id,
                    DataCriacao = DateTime.Now,
                    HistoricoId = addHistorico.Id
                };
                var historicosContas = _map.Map<HistoricosContas>(historicosContasDto);
                _repoHistoricosContas.Add(historicosContas);

                if(await _repoHistoricosContas.SaveAsync())
                {
                    return _map.Map<ContaDto>(conta);
                }
            }
            throw new ArgumentException("Erro ao persistir no banco");
        }
        public async Task<ContaDto> Transferencia(HelperContaDto contaDto)
        {
            // CONTA QUE IRA MANDAR A TRANSFERENCIA
            var contaOrigin = await _repo.GetByAgenciaAndContaCorrente(contaDto.Agencia, contaDto.Conta);
            // CONTA QUE IRA RECEBER A TRANSFERENCIA
            var contaDestino = await _repo.GetByAgenciaAndContaCorrente(contaDto.AgenciaDestino, contaDto.ContaDestino);

            // SE O REGISTRO RETORNA COMO NULL
            if (contaOrigin == null || contaDestino == null)
            {
                throw new NotFoundException("Conta origin nao encontrada e/ou conta destino nao encontrada!");
            }

            contaOrigin.Saldo -= contaDto.Quantia;
            contaDestino.Saldo += contaDto.Quantia;

            List<Conta> contas = new List<Conta>();
            contas.Add(contaOrigin);
            contas.Add(contaDestino);
            
            _repo.UpdateRange(contas);
            if (await _repo.SaveAsync())
            {
                HistoricoDto historicoOriginDto = new HistoricoDto
                {
                    DataTransacao = DateTime.Now,
                    DescricaoTransacao = "TRANSFERENCIA BANCARIA PARA CONTA CORRENTE",
                    DigitosAgencia = contaOrigin.DigitosAgencia,
                    DigitosConta = contaOrigin.DigitosConta,
                    DigitosAgenciaDestino = contaDestino.DigitosAgencia,
                    DigitosContaDestino = contaDestino.DigitosConta,
                    TipoConta = "CONTA CORRENTE",
                    Valor = contaDto.Quantia,
                    NomeConta = contaOrigin.NomeConta
                };
                var historicoAdd = await _historicoServices.Add(historicoOriginDto);

                HistoricoDto historicoDestinoDto = new HistoricoDto
                {
                    DataTransacao = DateTime.Now,
                    DescricaoTransacao = "RECEBIMENTO DE TRANSFERENCIA BANCARIA",
                    DigitosAgencia = contaOrigin.DigitosAgencia,
                    DigitosConta = contaOrigin.DigitosConta,
                    DigitosAgenciaDestino = contaDestino.DigitosAgencia,
                    DigitosContaDestino = contaDestino.DigitosConta,
                    TipoConta = "CONTA CORRENTE",
                    Valor = contaDto.Quantia,
                    NomeConta = contaDestino.NomeConta
                };
                var historicoDestinoAdd = await _historicoServices.Add(historicoDestinoDto);

                HistoricosContasDto historicosContasDestinoDto = new HistoricosContasDto
                {
                    ContaId = contaDestino.Id,
                    HistoricoId = historicoDestinoAdd.Id,
                    DataCriacao = DateTime.Now
                };

                HistoricosContasDto historicosContasDto = new HistoricosContasDto
                {
                    ContaId = contaOrigin.Id,
                    HistoricoId = historicoAdd.Id,
                    DataCriacao = DateTime.Now
                };
                var historicosContas = _map.Map<HistoricosContas>(historicosContasDto);
                var historicosContasDestino = _map.Map<HistoricosContas>(historicosContasDestinoDto);

                List<HistoricosContas> listHistoricosContas = new List<HistoricosContas>();

                listHistoricosContas.Add(historicosContas);
                listHistoricosContas.Add(historicosContasDestino);

                _repoHistoricosContas.AddRange(listHistoricosContas);

                if(await _repoHistoricosContas.SaveAsync()){
                    return _map.Map<ContaDto>(contaOrigin);
                }

            }
            throw new ArgumentException("Erro ao persistir no banco!");
        }
        // REALIZAR APLICACAO NA POUPANCA
        public async Task<ContaDto> AplicarPoupanca(HelperContaDto contaDto)
        {
            // CONTA CORRENTE
            var contaCorrente = await _repo.GetByAgenciaAndContaCorrente(contaDto.Agencia, contaDto.Conta);
            // CONTA POUPANCA
            var contaPoupanca = await _repo.GetByAgenciaAndContaPoupanca(contaDto.Agencia, contaDto.Conta);

            if(contaCorrente == null && contaPoupanca == null) 
            {
                throw new NotFoundException("Conta corrente e/ou Conta Poupanca não encontrada!");
            }

            contaCorrente.Saldo -= contaDto.Quantia;
            contaPoupanca.Saldo += contaDto.Quantia;
            
            List<Conta> contas = new List<Conta>();
            contas.Add(contaCorrente);
            contas.Add(contaPoupanca);

            _repo.UpdateRange(contas);
            if(await _repo.SaveAsync())
            {
                HistoricoDto historicoCorrenteDto = new HistoricoDto
                {
                    DataTransacao = DateTime.Now,
                    DescricaoTransacao = "APLICACAO DE VALOR NA CONTA POUPANCA",
                    DigitosAgencia = contaCorrente.DigitosAgencia,
                    DigitosConta = contaCorrente.DigitosConta,
                    DigitosAgenciaDestino = contaPoupanca.DigitosAgencia,
                    DigitosContaDestino = contaPoupanca.DigitosConta,
                    TipoConta = "CONTA CORRENTE",
                    Valor = contaDto.Quantia,
                    NomeConta = contaCorrente.NomeConta
                };
                var historicoContaCorrenteAdd = await _historicoServices.Add(historicoCorrenteDto);

                HistoricoDto historicoPoupancaDto = new HistoricoDto
                {
                    DataTransacao = DateTime.Now,
                    DescricaoTransacao = "RECEBIMENTO DE APLICACAO NA POUPANCA",
                    DigitosAgencia = contaCorrente.DigitosAgencia,
                    DigitosConta = contaCorrente.DigitosConta,
                    DigitosAgenciaDestino = contaPoupanca.DigitosAgencia,
                    DigitosContaDestino = contaPoupanca.DigitosConta,
                    TipoConta = "CONTA POUPANCA",
                    Valor = contaDto.Quantia,
                    NomeConta = contaPoupanca.NomeConta
                };
                var historicoContaPoupancaAdd = await _historicoServices.Add(historicoPoupancaDto);

                HistoricosContasDto historicosContasCorrenteDto = new HistoricosContasDto
                {
                    HistoricoId = historicoContaCorrenteAdd.Id,
                    ContaId = contaCorrente.Id,
                    DataCriacao = DateTime.Now
                };

                HistoricosContasDto historicosContasPoupancaDto = new HistoricosContasDto
                {
                    HistoricoId = historicoContaPoupancaAdd.Id,
                    ContaId = contaPoupanca.Id,
                    DataCriacao = DateTime.Now
                };
                var historicosContasCorrente = _map.Map<HistoricosContas>(historicosContasCorrenteDto);
                var historicosContasPoupanca = _map.Map<HistoricosContas>(historicosContasPoupancaDto);

                List<HistoricosContas> listHistoricosContas = new List<HistoricosContas>();

                listHistoricosContas.Add(historicosContasCorrente);
                listHistoricosContas.Add(historicosContasPoupanca);

                _repoHistoricosContas.AddRange(listHistoricosContas);
                if(await _repoHistoricosContas.SaveAsync())
                {
                    return _map.Map<ContaDto>(contaCorrente);
                }
            } 
            throw new ArgumentException("Erro ao persistir no banco de dados!");
        }
        // REALIZAR APLICACAO NA POUPANCA
        public async Task<ContaDto> ResgatarPoupanca(HelperContaDto contaDto)
        {
            // CONTA CORRENTE
            var contaCorrente = await _repo.GetByAgenciaAndContaCorrente(contaDto.Agencia, contaDto.Conta);
            // CONTA POUPANCA
            var contaPoupanca = await _repo.GetByAgenciaAndContaPoupanca(contaDto.Agencia, contaDto.Conta);

            if(contaCorrente == null && contaPoupanca == null) 
            {
                throw new NotFoundException("Conta corrente e/ou Conta Poupanca não encontrada!");
            }

            contaCorrente.Saldo += contaDto.Quantia;
            contaPoupanca.Saldo -= contaDto.Quantia;
            
            List<Conta> contas = new List<Conta>();
            contas.Add(contaCorrente);
            contas.Add(contaPoupanca);

            _repo.UpdateRange(contas);
            if(await _repo.SaveAsync())
            {
                 HistoricoDto historicoCorrenteDto = new HistoricoDto
                {
                    DataTransacao = DateTime.Now,
                    DescricaoTransacao = "RECEBIMENTO DE RESGATE DA POUPANCA",
                    DigitosAgencia = contaCorrente.DigitosAgencia,
                    DigitosConta = contaCorrente.DigitosConta,
                    DigitosAgenciaDestino = contaPoupanca.DigitosAgencia,
                    DigitosContaDestino = contaPoupanca.DigitosConta,
                    TipoConta = "CONTA CORRENTE",
                    Valor = contaDto.Quantia,
                    NomeConta = contaCorrente.NomeConta
                };
                var historicoContaCorrenteAdd = await _historicoServices.Add(historicoCorrenteDto);

                HistoricoDto historicoPoupancaDto = new HistoricoDto
                {
                    DataTransacao = DateTime.Now,
                    DescricaoTransacao = "RESGATE DE VALOR DA CONTA POUPANCA",
                    DigitosAgencia = contaCorrente.DigitosAgencia,
                    DigitosConta = contaCorrente.DigitosConta,
                    DigitosAgenciaDestino = contaPoupanca.DigitosAgencia,
                    DigitosContaDestino = contaPoupanca.DigitosConta,
                    TipoConta = "CONTA POUPANCA",
                    Valor = contaDto.Quantia,
                    NomeConta = contaPoupanca.NomeConta
                };
                var historicoContaPoupancaAdd = await _historicoServices.Add(historicoPoupancaDto);

                HistoricosContasDto historicosContasCorrenteDto = new HistoricosContasDto
                {
                    HistoricoId = historicoContaCorrenteAdd.Id,
                    ContaId = contaCorrente.Id,
                    DataCriacao = DateTime.Now
                };

                HistoricosContasDto historicosContasPoupancaDto = new HistoricosContasDto
                {
                    HistoricoId = historicoContaPoupancaAdd.Id,
                    ContaId = contaPoupanca.Id,
                    DataCriacao = DateTime.Now
                };
                var historicosContasCorrente = _map.Map<HistoricosContas>(historicosContasCorrenteDto);
                var historicosContasPoupanca = _map.Map<HistoricosContas>(historicosContasPoupancaDto);

                List<HistoricosContas> listHistoricosContas = new List<HistoricosContas>();

                listHistoricosContas.Add(historicosContasCorrente);
                listHistoricosContas.Add(historicosContasPoupanca);

                _repoHistoricosContas.AddRange(listHistoricosContas);
                if(await _repoHistoricosContas.SaveAsync())
                {
                    return _map.Map<ContaDto>(contaCorrente);
                }
            } 
            throw new ArgumentException("Erro ao persistir no banco de dados!");
        }
    }
}