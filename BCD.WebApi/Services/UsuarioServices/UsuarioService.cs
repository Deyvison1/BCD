using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using BCD.Domain.Entities.Identity;
using BCD.WebApi.Dtos;
using BCD.WebApi.Services.Exception;
using BCD.WebApi.Services.GenericServices;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace BCD.WebApi.Services.UsuarioServices
{
    public class UsuarioService
    {   
        private readonly IConfiguration _config;
        private readonly SignInManager<Usuario> _signInManager;
        private readonly IMapper _map;
        public readonly UserManager<Usuario> _userManager;
        private readonly RoleManager<Papel> _roleManager;
        private readonly GenericService _genericService;

        public UsuarioService(IConfiguration config, UserManager<Usuario> userManager,
            SignInManager<Usuario> signInManager, IMapper mapper, RoleManager<Papel> roleManager,
            GenericService genericService)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _genericService = genericService;
            _signInManager = signInManager;
            _map = mapper;
            _config = config;
        }
        // LISTAR TODOS USUARIOS
        public async Task<UsuarioDto[]> GetUser(UsuarioDto userDto)
        {
            var usuarios = await _userManager.Users.ToArrayAsync();

            var usuariosDto = _map.Map<UsuarioDto[]>(usuarios);
            return usuariosDto;
        }
        // REGISTRAR USUARIO
        public async Task<UsuarioDto> Registrar(UsuarioDto usuarioDto)
        {
            // MAPEAMENTO DTO
            var usuario = _map.Map<Usuario>(usuarioDto);
            // Criar Usuario
            var respostaCreateUser = await _userManager.CreateAsync(usuario, usuarioDto.Password);
            
            if(!respostaCreateUser.Succeeded)
            {
                throw new ArgumentException("Erro no CreateAsync, Usuario");
            }
            // Pegar Ultimo Usuario da Tabela
            var usuarioLast = await _genericService.lastAdd();
            
            var papelDtoNew = new PapelDto{ Name = usuarioLast.Papel, NormalizedName = usuarioLast.Papel };

            // BUSCAR PAPEL PELO NAME ASYNC
            var roleFindName = await _roleManager.RoleExistsAsync(papelDtoNew.Name);
            // MEPAMENTO DTO
            var papelAdd = _map.Map<Papel>(papelDtoNew);
            if(!roleFindName)
            {
                var respostaCreateRole =  await _roleManager.CreateAsync(papelAdd);
                
                // VERIFICO SE TEVE SUCESSO AO CRIAR PAPEL
                if(!respostaCreateRole.Succeeded)
                {
                    throw new ArgumentException("Erro no CreateAsync, Papel");
                }
            }
            // CRIAR PAPEL
            
            var papelLastDto = await _genericService.lastAddUsersRoles();
            var papelLast = _map.Map<Papel>(papelLastDto);

            Claim claim = new Claim("Conta", "Ler e Escrever");
            Claim claim2 = new Claim("Conta", "Ler");

            var respostaAddClaimsAsync = await _roleManager.AddClaimAsync(papelLast , claim);
            if(!respostaAddClaimsAsync.Succeeded)
            {
                throw new ArgumentException("Erro no CreateAsync, Claims");
            }
            
            return usuarioLast;
        }
        // LOGAR USUARIO
        public async Task<Usuario> Login(UsuarioLoginDto usuarioLoginDto)
        {
            var usuario = await _userManager.FindByEmailAsync(usuarioLoginDto.Email);

            var usuarioLogado = await _signInManager.CheckPasswordSignInAsync(usuario, usuarioLoginDto.Password, false);

            if(usuarioLogado.Succeeded) 
            {
                var appUser = await _userManager.Users.FirstOrDefaultAsync(u => u.Email == usuarioLoginDto.Email);

                var userLoginDto = _map.Map<Usuario>(appUser);

                return userLoginDto;
            }
            throw new UnauthorizedAccessException("Email ou Senha Incorretos!");
        }
        // GERAR TOKEN
        public async Task<string> GerarToken(Usuario usuario)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString()),
                new Claim(ClaimTypes.Email, usuario.Email),
                new Claim(ClaimTypes.Name, usuario.UserName)
            };

            var roles = await _userManager.GetRolesAsync(usuario);

            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role ));
            }

            var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes
                        (_config.GetSection("AppSettings:Token").Value));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddHours(8),
                SigningCredentials = creds
            };                     

            var tokenHandler = new JwtSecurityTokenHandler();

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
    }
}