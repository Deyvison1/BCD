using System.Threading.Tasks;
using BCD.Domain.Entities;

namespace BCD.Repository.EntitiesRepository.HistoricosContasRepository
{
    public interface IHistoricosContasRepository
    {
        void Add(HistoricosContas historicosContas);
        Task<bool> SaveAsync();
        Task<HistoricosContas[]> GetAll();
        Task<HistoricosContas> GetByHistoricoIdOrContaId(int id);
    }
}