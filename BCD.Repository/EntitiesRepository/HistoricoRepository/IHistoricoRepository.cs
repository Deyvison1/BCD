using System.Threading.Tasks;
using BCD.Domain.Entities;

namespace BCD.Repository.EntitiesRepository.HistoricoRepository
{
    public interface IHistoricoRepository
    {
        void Add(Historico historicoEntities);
        void AddRange(Historico[] entidades);
        void Update(Historico historicoEntities);
        void Delete(Historico historicoEntities);
        void DeleteRange(Historico[] historicoEntities);
        Task<bool> SaveAsync();
        Task<Historico[]> GetAllAsync();
        Task<Historico[]> GetByMesAsync(int mes, int agencia, int conta);
        Task<Historico> GetByIdAsync(int id);
        Task<Historico[]> GetBySearch(string search);
    }
}