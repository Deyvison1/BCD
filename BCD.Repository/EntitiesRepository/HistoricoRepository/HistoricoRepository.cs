using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BCD.Domain.Entities;
using BCD.Repository.Data;
using Microsoft.EntityFrameworkCore;

namespace BCD.Repository.EntitiesRepository.HistoricoRepository
{
    public class HistoricoRepository : IHistoricoRepository
    {
        private readonly BCDContext _context;

        public HistoricoRepository(BCDContext context)
        {
            _context = context;
        }
        public void Add(Historico historicoEntities)
        {
            _context.Add(historicoEntities);
        }
        public void AddRange(Historico[] entidades)
        {
            _context.AddRange(entidades);
        }
        public void Delete(Historico historicoEntities)
        {
            _context.Remove(historicoEntities);
        }

        public void DeleteRange(Historico[] historicoEntities)
        {
            _context.RemoveRange(historicoEntities);
        }
        public void Update(Historico historicoEntities)
        {
            _context.Update(historicoEntities);
        }
        public async Task<bool> SaveAsync()
        {
            return (await _context.SaveChangesAsync()) > 0;
        }
        // GETS
        // OBTER TODOS
        public async Task<Historico[]> GetAllAsync()
        {
            var getAll = _context.Historicos.OrderByDescending(
                x => x.Id
            ).Include(x => x.Contas).ThenInclude(x => x.Conta)
            .AsNoTracking().ToArrayAsync();
            return await getAll;
        }
        // OBTER POR ID
        public async Task<Historico> GetByIdAsync(int id)
        {
            var getById = _context.Historicos.AsNoTracking().FirstOrDefaultAsync(
                x => x.Id.Equals(id)
            );
            return await getById;
        }
        // OBTER POR PESQUISA, VALOR OU DESCRICAO OU DIGITOS CONTA
        public async Task<Historico[]> GetBySearch(string search)
        {
            var getBySearch = _context.Historicos.Where(
                x => x.Valor.ToString().Contains(search) || x.DescricaoTransacao.ToLower().Contains(search.ToLower())
                || x.DigitosConta.ToString().Contains(search)
            ).AsNoTracking().ToArrayAsync();
            return await getBySearch;
        }

        public async Task<Historico[]> GetByMesCorrenteAsync(int mes, int agencia, int conta)
        {
            return await _context.Historicos.Where(
                x => x.TipoConta == 0 && x.Operacao > 0 && x.DataTransacao.Month.Equals(mes)
            ).OrderByDescending(x => x.DataTransacao).AsNoTracking().ToArrayAsync();
        }
        public async Task<Historico[]> GetByMesPoupancaAsync(int mes, int agencia, int conta)
        {
            return await _context.Historicos.Where(
                x => x.TipoConta != 0 && x.Operacao > 0 && x.DataTransacao.Month.Equals(mes)
            ).OrderByDescending(x => x.DataTransacao).AsNoTracking().ToArrayAsync();
        }

        public async Task<Historico[]> GetLastMesesPoupanca(int agencia, int conta, int tipoConta, params int[] meses)
        {
            var listaHistoricosByLastMeses = await _context.Historicos.Where(
                list => meses.Contains(list.DataTransacao.Month) 
                && list.DigitosAgencia.Equals(agencia)
                && list.DigitosConta.Equals(conta)
                && list.TipoConta != 0
            ).OrderByDescending(
                x => x.DataTransacao
            ).AsNoTracking().ToArrayAsync();
            return listaHistoricosByLastMeses;
        }
        public async Task<Historico[]> GetLastMesesCorrente(int agencia, int conta, int tipoConta, params int[] meses)
        {
            var listaHistoricosByLastMeses = await _context.Historicos.Where(
                list => meses.Contains(list.DataTransacao.Month) 
                && list.DigitosAgencia.Equals(agencia)
                && list.DigitosConta.Equals(conta)
                && list.TipoConta == 0
            ).OrderByDescending(
                x => x.DataTransacao
            ).AsNoTracking().ToArrayAsync();
            return listaHistoricosByLastMeses;
        }
    }
}