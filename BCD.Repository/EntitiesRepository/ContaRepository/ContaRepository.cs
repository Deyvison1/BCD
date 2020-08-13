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
            ).Include(x => x.Extrato).ThenInclude(x => x.Historico)
            .Include(x => x.Pessoa)
            .ToArrayAsync();
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
        // OBTER PELO ID DA PESSOA
        public async Task<Conta> GetByIdPessoaAsync(int idPessoa)
        {
            var getByIdPessoa = _context.Contas.FirstOrDefaultAsync(
                x => x.PessoaId.Equals(idPessoa)
            );
            return await getByIdPessoa;
        }
        // OBTER POR PESQUISA, AGENCIA E CONTA
        public async Task<Conta[]> GetByAgenciaAndConta(int agencia, int conta)
        {
            var getByAgenciaAndConta = _context.Contas.Where(
                x => x.DigitosAgencia.Equals(agencia) && x.DigitosConta.Equals(conta) 
            ).ToArrayAsync();
            return await getByAgenciaAndConta; 
        }
    }
}