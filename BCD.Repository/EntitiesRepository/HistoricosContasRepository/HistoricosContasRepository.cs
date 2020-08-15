using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BCD.Domain.Entities;
using BCD.Repository.Data;
using Microsoft.EntityFrameworkCore;

namespace BCD.Repository.EntitiesRepository.HistoricosContasRepository
{
    public class HistoricosContasRepository : IHistoricosContasRepository
    {
        private readonly BCDContext _context;
        public HistoricosContasRepository(BCDContext context)
        {
            _context = context;

        }
        public void Add(HistoricosContas historicosContas)
        {
            _context.Add(historicosContas);
        }
        public void AddRange(IList<HistoricosContas> historicosContas)
        {
            _context.AddRange(historicosContas);
        }
        public async Task<bool> SaveAsync()
        {
            return (await _context.SaveChangesAsync()) > 0;
        }
        public async Task<HistoricosContas[]> GetAll()
        {
            return await _context.HistoricosContas.OrderByDescending(
                x => x.DataCriacao
            ).ToArrayAsync();
        }

        public async Task<HistoricosContas> GetByHistoricoIdOrContaId(int id)
        {
            return await _context.HistoricosContas.FirstOrDefaultAsync(
                x => x.HistoricoId.Equals(id) || x.ContaId.Equals(id)
            );
        }
    }
}