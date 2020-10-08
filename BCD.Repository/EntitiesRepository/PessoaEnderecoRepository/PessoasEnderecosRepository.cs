using System.Linq;
using System.Threading.Tasks;
using BCD.Domain.Entities;
using BCD.Repository.Data;
using Microsoft.EntityFrameworkCore;

namespace BCD.Repository.EntitiesRepository.PessoaEnderecoRepository
{
    public class PessoasEnderecosRepository : IPessoasEnderecosRepository
    {
        public BCDContext _context { get; }
        public PessoasEnderecosRepository(BCDContext context)
        {
            _context = context;
        }
        // ADICIONAR UM
        public void Add<T>(T entidade) where T : class
        {
            _context.Add(entidade);
        }
        // ADICIONAR MAIS DE UM
        public void AddRange<T>(T[] entidades) where T : class
        {
            _context.AddRangeAsync(entidades);
        }
        // LISTAR TODOS
        public async Task<PessoasEnderecos[]> GetAll()
        {
            return await _context.PessoasEnderecos.OrderBy(x => x.Criacao).AsNoTracking().ToArrayAsync();
        }

        public async Task<bool> SaveChangeAsync()
        {
            return (await _context.SaveChangesAsync()) > 0;
        }
    }
}