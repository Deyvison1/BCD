using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using BCD.Domain.Entities.Identity;

namespace BCD.Repository.EntitiesRepository.PapeisUsuariosRepository
{
    public interface IRepositoryGeneric
    {
        void add<T>(T entidade) where T : class;
        Task<bool> SaveChangeAsync();
        Task<Usuario> lastAdd();
        Task<UsuariosPapeis> lastAddUsersRoles();

    }
}