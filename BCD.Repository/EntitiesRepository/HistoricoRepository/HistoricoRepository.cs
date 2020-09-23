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
            .ToArrayAsync();
            return await getAll;
        }
        // OBTER POR ID
        public async Task<Historico> GetByIdAsync(int id)
        {
            var getById = _context.Historicos.FirstOrDefaultAsync(
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
            ).ToArrayAsync();
            return await getBySearch;
        }

        public async Task<Historico[]> GetByMesAsync(int mes)
        {
            return await _context.Historicos.Where(
                x => x.DataTransacao.Month.Equals(mes)
            ).ToArrayAsync();
        }
    }
}