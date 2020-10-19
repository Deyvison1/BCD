using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using BCD.Domain.Entities.Identity;
using BCD.WebApi.Dtos;
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
        public UsuarioService(IConfiguration config, UserManager<Usuario> userManager,
            SignInManager<Usuario> signInManager, IMapper mapper)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _map = mapper;
            _config = config;
        }
        // LISTAR TODOS USUARIOS
        public UsuarioDto GetUser(UsuarioDto userDto)
        {
            return userDto;
        }
        // REGISTRAR USUARIO
        public async Task<UsuarioDto> Registrar(UsuarioDto usuarioDto)
        {
                var usuario = _map.Map<Usuario>(usuarioDto);

                var resultUsuarioAdd = await _userManager.CreateAsync(usuario, usuarioDto.Password);

                var userToReturn = _map.Map<UsuarioDto>(usuario);

                if(resultUsuarioAdd.Succeeded)
                {
                    return userToReturn;
                }
                throw new ArgumentException($"{resultUsuarioAdd.Errors}");
        }
        // LOGAR USUARIO
        public async Task<Usuario> Login(UsuarioLoginDto usuarioLoginDto)
        {
            var usuario = await _userManager.FindByEmailAsync(usuarioLoginDto.Email);

            var usuarioLogado = await _signInManager.CheckPasswordSignInAsync(usuario, usuarioLoginDto.Password, false);

            if(usuarioLogado.Succeeded) 
            {
                var appUser = await _userManager.Users.FirstOrDefaultAsync(u => u.NormalizedEmail == usuarioLoginDto.Email.ToUpper());

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