using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BCD.Domain.Entities;

namespace BCD.Repository.EntitiesRepository.EnderecosPessoasRepository
{
    public interface IEnderecosPessoasRepository
    {
        void Add(EnderecosPessoas enderecosPessoas);
        void AddRange(List<EnderecosPessoas> enderecoPessoas);
        Task<bool> SaveAsync();
        Task<bool> ExisteEndereco(int idEndereco, int pessoaId);
        Task<EnderecosPessoas[]> GetAll();
        Task<EnderecosPessoas[]> GetByEnderecoIdOrPessoaId(int id);
    }
}
