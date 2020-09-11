using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using BCD.Domain.Entities;

namespace BCD.Repository.EntitiesRepository.ContaRepository
{
    public interface IContaRepository
    {
        void Add(Conta contaEntites);
        void Update(Conta contaEntites);
        void UpdateRange(List<Conta> contas);
        void Delete(Conta contaEntites);
        void DeleteRange(Conta[] contaEntites);
        Task<bool> SaveAsync();
        Task<bool> ExisteContaPoupanca(int idPessoa);
        Task<bool> ExisteContaCorrente(int idPessoa);
        Task<Conta[]> ListGetByIdPessoaAsync(int idPessoa);
        Task<Conta> GetByIdPessoaAsync(int idPessoa);
        Task<Conta> GetByIdPessoaCorrenteAsync(int idPessoa);
        Task<Conta> GetByAgenciaAndContaCorrente(int agencia, int conta);
        Task<Conta> GetByAgenciaAndContaPoupanca(int agencia, int conta);
        Task<Conta[]> GetAllAsync();
        Task<Conta[]> GetByIdAsyncList(int id);
        Task<Conta> GetByIdAsync(int id);
        Task<Conta[]> GetBySearchAsync(string search);
        Task<bool> ExistContaFindByCpf(int cpf, int agencia, int conta);

    }
}