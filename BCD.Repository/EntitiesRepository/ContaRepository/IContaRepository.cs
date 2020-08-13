using System.Threading.Tasks;
using BCD.Domain.Entities;

namespace BCD.Repository.EntitiesRepository.ContaRepository
{
    public interface IContaRepository
    {
        void Add(Conta contaEntites);
        void Update(Conta contaEntites);
        void Delete(Conta contaEntites);
        void DeleteRange(Conta[] contaEntites);
        Task<bool> SaveAsync();
        Task<Conta> GetByIdPessoaAsync(int idPessoa);
        Task<Conta[]> GetByAgenciaAndConta(int agencia, int conta);
        Task<Conta[]> GetAllAsync();
        Task<Conta> GetByIdAsync(int id);
        Task<Conta[]> GetBySearchAsync(string search);
    }
}