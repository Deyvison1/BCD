using System.Collections.Generic;
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
        Task<Historico[]> GetLastMesesPoupanca(int agencia, int conta, int tipoConta, params int[] meses);
        Task<Historico[]> GetLastMesesCorrente(int agencia, int conta, int tipoConta, params int[] meses);
        Task<Historico[]> GetByMesCorrenteAsync(int mes, int agencia, int conta);
        Task<Historico[]> GetByMesPoupancaAsync(int mes, int agencia, int conta);
        Task<Historico> GetByIdAsync(int id);
        Task<Historico[]> GetBySearch(string search);
    }
}