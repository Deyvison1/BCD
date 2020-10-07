using System.Threading.Tasks;
using BCD.Domain.Entities;

namespace BCD.Repository.EntitiesRepository.PessoaRepository
{
    public interface IPessoaRepository
    {
        void Add(Pessoa pessoaEntities);
        void Update(Pessoa pessoaEntities);
        void Delete(Pessoa pessoaEntities);
        void DeleteRange(Pessoa[] pessoaEntities);
        Task<bool> SaveAsync();
        Task<bool> ExisteCPF(string cpf);
        Task<Pessoa[]> GetAllPessoaById(int idPessoa);
        Task<Pessoa[]> GetAllAsync();
        Task<string> GetCpfByIdPessoa(int idPessoa);
        Task<Pessoa> GetAllPessoaByIdAsync(int idPessoa);
        Task<Pessoa> GetByIdAsync(int id);
        Task<Pessoa[]> GetBySearchAsync(string search);
    }
}