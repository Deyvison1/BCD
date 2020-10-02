using System.Collections.Generic;
using System.Threading.Tasks;
using BCD.Domain.Entities;

namespace BCD.Repository.EntitiesRepository.EnderecoRepository
{
    public interface IEnderecoRepository
    {
        void Add(Endereco enderecoEntities);
        void AddRange(List<Endereco> enderecos);
        void Update(Endereco enderecoEntities);
        void Delete(Endereco enderecoEntities);
        void DeleteRange(Endereco[] enderecoEntities);
        Task<bool> SaveAsync();
        Task<int[]> ExisteCepCadastrado(List<int> ceps);
        Task<Endereco> GetByCep(int cep);
        Task<Endereco[]> GetAllAsync();
        Task<Endereco> GetByIdAsync(int id);
        Task<Endereco[]> GetBySearchAsync(string search);

    }
}