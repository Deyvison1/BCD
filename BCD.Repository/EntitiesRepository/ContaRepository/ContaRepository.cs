using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BCD.Domain.Entities;
using BCD.Repository.Data;
using Microsoft.EntityFrameworkCore;

namespace BCD.Repository.EntitiesRepository.ContaRepository
{
    public class ContaRepository : IContaRepository
    {
        // DATA CONTEXT
        private BCDContext _context { get; }
        public ContaRepository(BCDContext context)
        {
            _context = context;
        }
        public void UpdateRange(List<Conta> contas)
        {
            _context.UpdateRange(contas);
        }
        public void Add(Conta contaEntitie)
        {
            _context.Add(contaEntitie);
        }
        public void Delete(Conta contaEntites)
        {
            _context.Remove(contaEntites);
        }
        public void DeleteRange(Conta[] contaEntites)
        {
            _context.RemoveRange(contaEntites);
        }
        public void Update(Conta contaEntites)
        {
            _context.Update(contaEntites);
        }
        public async Task<bool> SaveAsync()
        {
            return (await _context.SaveChangesAsync()) > 0;
        }
        // GETS
        // OBTER TODOS
        public async Task<Conta[]> GetAllAsync()
        {
            var getAllAsync = _context.Contas.OrderByDescending(
                x => x.Id
            ).ToArrayAsync();
            return await getAllAsync;
        }
        // OBTER PELO ID
        public async Task<Conta> GetByIdAsync(int id)
        {
            var getById = _context.Contas.FirstOrDefaultAsync(
                x => x.Id.Equals(id)
            );
            return await getById;
        }
        // OBTER PELA PESQUISA POR NOME
        public async Task<Conta[]> GetBySearchAsync(string nome)
        {
            var getBySearch = _context.Contas.Where(
                x => x.NomeConta.ToLower().Contains(nome.ToLower())
            ).ToArrayAsync();
            return await getBySearch;
        }
        // OBTER LISTA PELO ID DA PESSOA
        public async Task<Conta[]> ListGetByIdPessoaAsync(int idPessoa)
        {
            var conta = _context.Contas.Where(
                x => x.PessoaId.Equals(idPessoa)
            ).Include(x => x.Extrato).ToArrayAsync();
            return await conta;
        }
        // OBTER PELO ID PESSOA
        public async Task<Conta> GetByIdPessoaCorrenteAsync(int idPessoa)
        {
            var conta = _context.Contas.FirstOrDefaultAsync(
                x => x.PessoaId.Equals(idPessoa) && x.TipoConta == 0
            );
            return await conta;
        }
        // OBTER POR AGENCIA E CONTA CORRENTE
        public async Task<Conta> GetByAgenciaAndContaCorrente(int agencia, int conta)
        {
            var getByAgenciaAndContaConrrente = await _context.Contas.FirstOrDefaultAsync(
                x => x.DigitosAgencia.Equals(agencia) && x.DigitosConta.Equals(conta)
                && x.TipoConta == 0
            );
            return getByAgenciaAndContaConrrente;
        }
        // OBTER POR AGENCIA E CONTA POUPANCA
        public async Task<Conta> GetByAgenciaAndContaPoupanca(int agencia, int conta)
        {
            var getByAgenciaAndContaPoupanca = await _context.Contas.FirstOrDefaultAsync(
                x => x.DigitosAgencia.Equals(agencia) && x.DigitosConta.Equals(conta)
                && x.TipoConta != 0
            );
            return getByAgenciaAndContaPoupanca;
        }

        public async Task<bool> ExisteContaCorrente(int idPessoa)
        {
            return await _context.Contas.AnyAsync(
                x => x.PessoaId == idPessoa && x.TipoConta == 0
            );
        }

        public async Task<bool> ExisteContaPoupanca(int idPessoa)
        {
            return await _context.Contas.AnyAsync(
                x => x.PessoaId == idPessoa && x.TipoConta != 0
            );
        }

        public async Task<Conta> GetByIdPessoaAsync(int idPessoa)
        {
            return await _context.Contas.FirstOrDefaultAsync(
                x => x.PessoaId.Equals(idPessoa)
            );
        }

        public async Task<Conta[]> GetByIdAsyncList(int id)
        {
            return await _context.Contas.Where(
                x => x.Id.Equals(id)
            ).Include(
                x => x.Extrato
            ).ThenInclude(
                x => x.Historico
            ).ToArrayAsync();
        }

        public async Task<bool> ExistContaFindByCpf(string cpf, int agencia, int conta)
        {
            return (await _context.Contas.AnyAsync(
                x => x.DigitosAgencia.Equals(agencia) && x.DigitosConta.Equals(conta)
                && x.CPF.Equals(cpf)
            ));
        }

        public async Task<Conta[]> GetByListIdConta(List<int> idConta)
        {
            // Crio uma lista do tipo conta
            List<Conta> list = new List<Conta>();
            // Percorro a lista de idConta
            foreach(int item in idConta)
            {
                // Cada vez que percorrer a lista eu ja busco a conta pelo id
                var conta = await _context.Contas.FirstOrDefaultAsync(
                    x => x.Id.Equals(item)
                );
                // Cada vez que fizer a busca pelo id vou adicionando a resposta a lista
                list.Add(conta);
            }
            // retorno a lista
            return list.ToArray();
        }

        public async Task<Conta> GetStatusContaByCpfAndSenha(string cpf)
        {
            var conta = await _context.Contas.FirstOrDefaultAsync(
                x => x.CPF.Equals(cpf)
            );
            return conta;
        }
    }
}