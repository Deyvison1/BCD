using System.Linq;
using System.Threading.Tasks;
using BCD.Domain.Entities;
using BCD.Repository.Data;
using Microsoft.EntityFrameworkCore;

namespace BCD.Repository.EntitiesRepository.PessoaRepository
{
    public class PessoaRepository : IPessoaRepository
    {
        private readonly BCDContext _context;

        public PessoaRepository(BCDContext context)
        {
            _context = context;
        }
        public void Add(Pessoa pessoaEntities)
        {
            _context.Pessoas.Add(pessoaEntities);
        }

        public void Delete(Pessoa pessoaEntities)
        {
            _context.Remove(pessoaEntities);
        }
        public void Update(Pessoa pessoaEntities)
        {
            _context.Update(pessoaEntities);
        }
        public void DeleteRange(Pessoa[] pessoaEntities)
        {
            _context.RemoveRange(pessoaEntities);
        }
        public async Task<bool> SaveAsync()
        {
            return (await _context.SaveChangesAsync()) > 0;
        }
        // GETS
        // OBTER TODOS
        public async Task<Pessoa[]> GetAllAsync()
        {
            var getAll = _context.Pessoas.OrderByDescending(
                x => x.Id
            ).Include(enderecos => enderecos.Enderecos).ThenInclude(endereco => endereco.Endereco).
            Include(x => x.Contas).AsNoTracking()
            .ToArrayAsync();
            return await getAll;
        }
        
        // OBTER POR ID
        public async Task<Pessoa> GetByIdAsync(int id)
        {
            IQueryable<Pessoa> query = _context.Pessoas.Include(enderecos => enderecos.Enderecos)
            .ThenInclude(endereco => endereco.Endereco).Include(contas => contas.Contas);

            query = query.AsNoTracking().OrderByDescending(pessoa => pessoa.Id).Where(
                pessoa => pessoa.Id.Equals(id)
            );
            return await query.FirstOrDefaultAsync();
        }
        // OBTER POR PESQUISA, NOME OU CPF
        public async Task<Pessoa[]> GetBySearchAsync(string search)
        {
            var getBySearch = _context.Pessoas.Where(
                x => x.Nome.ToLower().Contains(search.ToLower()) || x.CPF.ToString().Contains(search)
            ).AsNoTracking().ToArrayAsync();
            return await getBySearch;
        }

        public async Task<bool> ExisteCPF(string cpf)
        {
            return (await _context.Pessoas.AnyAsync( x => x.CPF.Equals(cpf)));
        }

        public async Task<Pessoa[]> GetAllPessoaById(int idPessoa)
        {
            var pessoas = _context.Pessoas.Where(
                x => x.Id.Equals(idPessoa)
            ).Include(x => x.Enderecos).Include(x => x.Contas).AsNoTracking().ToArrayAsync();
            return await pessoas;
        }

        public async Task<string> GetCpfByIdPessoa(int idPessoa)
        {
            string[] cpf = await _context.Pessoas.Where(
                x => x.Id.Equals(idPessoa)
            ).Select(x => x.CPF).ToArrayAsync();
            string cpfFinal = cpf.FirstOrDefault();
            return cpfFinal;
        }

        public async Task<Pessoa> GetAllPessoaByIdAsync(int idPessoa)
        {
            IQueryable<Pessoa> query = _context.Pessoas.
                Include(x => x.Contas).
                Include(x => x.Enderecos);

            query = query.AsNoTracking().
            OrderByDescending(x => x.Id).
            Where(x => x.Id.Equals(idPessoa));
            return await query.FirstOrDefaultAsync();    
        }
    }
}