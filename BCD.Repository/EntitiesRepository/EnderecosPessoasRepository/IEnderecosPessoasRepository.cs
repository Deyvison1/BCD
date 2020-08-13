using System;
using System.Threading.Tasks;
using BCD.Domain.Entities;

namespace BCD.Repository.EntitiesRepository.EnderecosPessoasRepository
{
    public interface IEnderecosPessoasRepository
    {
        void Add(EnderecosPessoas enderecosPessoas);
        Task<bool> SaveAsync();
        Task<EnderecosPessoas[]> GetAll();
        Task<EnderecosPessoas[]> GetByEnderecoIdOrPessoaId(int id);
    }
}
