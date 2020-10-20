using System;
using System.Threading.Tasks;
using AutoMapper;
using BCD.Domain.Entities.Identity;
using BCD.WebApi.Dtos;
using BCD.WebApi.Services.UsuarioServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace BCD.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        private readonly UsuarioService _service;

        public UsuarioController(UsuarioService service)
        {
            _service = service;
        }
        // LISTAR TODOS USUARIOS
        [HttpGet("GetUser")]
        public async Task<IActionResult> GetUser(UsuarioDto userDto)
        {
            try 
            {
                var usuarios = await _service.GetUser(userDto);

                return Ok(usuarios);
            }
            catch(ArgumentException e)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"{e.Message}");
            }
        }
        // REGISTRAR USUARIO
        [HttpPost("Registrar")]
        [AllowAnonymous]
        public async Task<IActionResult> Registrar(UsuarioDto usuarioDto)
        {
            try 
            {
                var usuarioAdd = await _service.Registrar(usuarioDto);

                return Created("GetUser", usuarioAdd);
            }
            catch(ArgumentException e) 
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"{e.Message}");
            }
        }

        [HttpPost("Login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login(UsuarioLoginDto usuarioLoginDto)
        {
            try
            {
                var usuarioLogado = await _service.Login(usuarioLoginDto);

                return Ok(new {
                    token = _service.GerarToken(usuarioLogado).Result,
                    usuario = usuarioLoginDto
                }
                );
            }
            catch(UnauthorizedAccessException e) 
            {
                return Unauthorized($"{e.Message}");
            }
            catch(ArgumentException e) 
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"{e.Message}");
            }
        }
    }
}