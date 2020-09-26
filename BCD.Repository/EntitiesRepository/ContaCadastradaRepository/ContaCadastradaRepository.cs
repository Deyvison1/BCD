using System.Linq;
using System.Threading.Tasks;
using BCD.Domain.Entities;
using BCD.Repository.Data;
using Microsoft.EntityFrameworkCore;

namespace BCD.Repository.EntitiesRepository.ContaCadastradaRepository
{
    public class ContaCadastradaRepository : IContaCadastradaRepository
    {
        private readonly BCDContext _context;

        public ContaCadastradaRepository(BCDContext context)
        {
            _context = context;
        }

        public void Add(ContaCadastrada contaCadastrada)
        {
            _context.Add(contaCadastrada);
        }

        public async Task<bool> ExisteContaCadastrada(int contaId, int pessoaId)
        {
            bool resposta = await _context.ContasCadastradas.AnyAsync(
                x => x.ContaId.Equals(contaId) && x.PessoaId.Equals(pessoaId)
            );
            return resposta;
        }

        public async Task<ContaCadastrada[]> GetAllAsync()
        {
            return await _context.ContasCadastradas.ToArrayAsync();
        }

        public async Task<ContaCadastrada[]> GetAllByPessoaIdAsync(int pessoaId)
        {
            return await _context.ContasCadastradas.Where(
                x => x.PessoaId.Equals(pessoaId)
            ).ToArrayAsync();
        }

        public async Task<bool> SaveChangeAsync()
        {
            return (await _context.SaveChangesAsync()) > 0;
        }
    }
}