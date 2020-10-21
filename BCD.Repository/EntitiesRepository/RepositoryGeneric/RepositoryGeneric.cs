using System.Threading.Tasks;
using BCD.Repository.Data;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System;
using BCD.Domain.Entities.Identity;

namespace BCD.Repository.EntitiesRepository.PapeisUsuariosRepository
{
    public class RepositoryGeneric : IRepositoryGeneric
    {
        private readonly BCDContext _context;

        public RepositoryGeneric(BCDContext context)
        {
            _context = context;
        }
        public void add<T>(T entidade) where T : class
        {
            _context.Add(entidade);
        }

        public async Task<Usuario> lastAdd()
        {
            return await _context.Users.LastOrDefaultAsync();
        }

        public async Task<UsuariosPapeis> lastAddUsersRoles()
        {
            return await _context.UserRoles.LastOrDefaultAsync();
        }

        public async Task<bool> SaveChangeAsync()
        {
            return (await _context.SaveChangesAsync()) > 0;
        }
    }
}