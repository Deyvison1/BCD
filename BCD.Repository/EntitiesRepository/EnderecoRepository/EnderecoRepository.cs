using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BCD.Domain.Entities;
using BCD.Repository.Data;
using Microsoft.EntityFrameworkCore;

namespace BCD.Repository.EntitiesRepository.EnderecoRepository
{
    public class EnderecoRepository : IEnderecoRepository
    {
        private readonly BCDContext _context;

        public EnderecoRepository(BCDContext context)
        {
            _context = context;
        }

        public async void AddRange(List<Endereco> enderecos)
        {
            await _context.AddRangeAsync(enderecos);
        }
        public void Add(Endereco enderecoEntities)
        {
            _context.Add(enderecoEntities);
        }

        public void Delete(Endereco enderecoEntities)
        {
            _context.Remove(enderecoEntities);
        }

        public void DeleteRange(Endereco[] enderecoEntities)
        {
            _context.RemoveRange(enderecoEntities);
        }

        public void Update(Endereco enderecoEntities)
        {
            _context.Update(enderecoEntities);
        }
        public async Task<bool> SaveAsync()
        {
            return (await _context.SaveChangesAsync()) > 0;
        }
        // GETS
        // OBTER TODOS
        public async Task<Endereco[]> GetAllAsync()
        {
            var getAll = _context.Enderecos.OrderByDescending(
                x => x.Id
            ).ToArrayAsync();
            return await getAll;
        }
        // OBTER PELO ID
        public async Task<Endereco> GetByIdAsync(int id)
        {
            var getById = _context.Enderecos.FirstOrDefaultAsync(
                x => x.Id.Equals(id)
            );
            return await getById;
        }
        // OBTER POR PESQUISA, CEP OU BAIRRO
        public async Task<Endereco[]> GetBySearchAsync(string search)
        {
            var getBySearch = _context.Enderecos.Where(
                x => x.CEP.ToString().Contains(search) || x.Bairro.ToLower().Contains(search.ToLower()) 
            ).ToArrayAsync();
            return await getBySearch;
        }

        public async Task<Endereco> GetByCep(string cep)
        {
            return await _context.Enderecos.FirstOrDefaultAsync(
                x => x.CEP.Equals(cep)
            );
        }
        // Se retornar algum registro e pq tem esse cep cadastrado ja
        public async Task<int[]> ExisteCepCadastrado(List<string> ceps)
        {
            var result = _context.Enderecos.Where(
                list => ceps.Contains(list.CEP)
            ).Select(x => x.Id);
            return await result.ToArrayAsync();
        }
    }
}