using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BCD.Domain.Entities;
using BCD.Repository.Data;
using Microsoft.EntityFrameworkCore;

namespace BCD.Repository.EntitiesRepository.EnderecosPessoasRepository
{
    public class EnderecosPessoasRepository : IEnderecosPessoasRepository
    {
        public BCDContext _context { get; }
        public EnderecosPessoasRepository(BCDContext context)
        {
            _context = context;
        }

        public async void AddRange(List<EnderecosPessoas> enderecoPessoas)
        {
            await _context.AddRangeAsync(enderecoPessoas);
        }
        public void Add(EnderecosPessoas enderecosPessoas)
        {
            _context.Add(enderecosPessoas);
        }

        public async Task<EnderecosPessoas[]> GetAll()
        {
            return await _context.EnderecosPessoas.OrderBy(
                    x => x.DataAtualizacao
                ).ToArrayAsync();
        }

        public async Task<EnderecosPessoas[]> GetByEnderecoIdOrPessoaId(int id)
        {
            var getById = _context.EnderecosPessoas.Where(
                    x => x.EnderecoId.Equals(id) || x.PessoaId.Equals(id)
                ).ToArrayAsync();
            return await getById;
        }

        public async Task<bool> SaveAsync()
        {
            return (await _context.SaveChangesAsync()) > 0;
        }

        public async Task<bool> ExisteEndereco(int idEndereco, int pessoaId)
        {
            return await _context.EnderecosPessoas.AnyAsync(
                x => x.PessoaId.Equals(pessoaId) && x.EnderecoId.Equals(idEndereco)
            );
        }

    }
}
