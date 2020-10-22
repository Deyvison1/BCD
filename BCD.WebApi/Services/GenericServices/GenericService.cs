using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using BCD.Domain.Entities.Identity;
using BCD.Repository.EntitiesRepository.PapeisUsuariosRepository;
using BCD.WebApi.Dtos;
using BCD.WebApi.Services.Exception;

namespace BCD.WebApi.Services.GenericServices
{
    public class GenericService
    {
        private readonly IRepositoryGeneric _repoGeneric;

        public GenericService(IMapper _map) 
        {
            this._map = _map;
               
        }
                private IMapper _map { get; }

        public GenericService(IRepositoryGeneric repoGeneric, IMapper map)
        {
            _repoGeneric = repoGeneric;
            _map = map;
        }
        
        public async Task<T> add<T>(T entidadeParametro) where T : class
        {
            _repoGeneric.add(entidadeParametro);

            if(await _repoGeneric.SaveChangeAsync())
            {
                return entidadeParametro;
            }
            throw new ArgumentException("Erro ao persistir dados");
        }
        public async Task<bool> addUsersRoles(UserRolesDto entidadeDto)
        {
            var entidade = _map.Map<UsuariosPapeis>(entidadeDto);
            _repoGeneric.add(entidade);

            if(await _repoGeneric.SaveChangeAsync())
            {
                return true;
            }
            return false;
        }
        // PEGAR ULTIMO REGISTRO DA TABELA USUARIOS
        public async Task<UsuarioDto> lastAdd()
        {
            var usuarioLast = await _repoGeneric.lastAdd();

            var usuarioLastDto = _map.Map<UsuarioDto>(usuarioLast);

            return usuarioLastDto;
        }
        // PEGAR ULTIMO REGISTRO DA TABELA PAPEIS
        public async Task<PapelDto> lastAddPapel()
        {
            var papel = await _repoGeneric.lastAddPapel();

            var papelDto = _map.Map<PapelDto>(papel);

            return papelDto;
        }
        // PEGAR ULTIMO REGISTRO DA TABELA USUARIOSPAPEIS
        public async Task<UserRolesDto> lastAddUsersRoles()
        {
            var usuariosPapeisLast = await _repoGeneric.lastAddUsersRoles();

            var usuarioPapeisLastDto = _map.Map<UserRolesDto>(usuariosPapeisLast);

            return usuarioPapeisLastDto;
        }
    }
}