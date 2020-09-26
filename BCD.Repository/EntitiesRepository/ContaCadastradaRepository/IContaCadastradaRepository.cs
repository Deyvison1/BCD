using System.Threading.Tasks;
using BCD.Domain.Entities;

namespace BCD.Repository.EntitiesRepository.ContaCadastradaRepository
{
    public interface IContaCadastradaRepository
    {
        Task<ContaCadastrada[]> GetAllAsync();
        Task<bool> ExisteContaCadastrada(int contaId, int pessoaId);
        Task<ContaCadastrada[]> GetAllByPessoaIdAsync(int pessoaId);
        Task<bool> SaveChangeAsync();
        void Add(ContaCadastrada contaCadastrada);
    }
}