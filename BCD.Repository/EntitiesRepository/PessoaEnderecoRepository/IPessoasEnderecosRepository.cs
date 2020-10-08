using System.Threading.Tasks;
using BCD.Domain.Entities;

namespace BCD.Repository.EntitiesRepository.PessoaEnderecoRepository
{
    public interface IPessoasEnderecosRepository
    {
         void Add<T>(T entidade) where T : class;
         void AddRange<T>(T[] entidades) where T : class;
         Task<bool> SaveChangeAsync();
         Task<PessoasEnderecos[]> GetAll();
    }
}