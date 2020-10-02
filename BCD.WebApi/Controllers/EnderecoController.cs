using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BCD.WebApi.Dtos;
using BCD.WebApi.Services.EnderecoServices;
using BCD.WebApi.Services.Exception;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BCD.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EnderecoController : ControllerBase
    {
        private readonly EnderecoService _service;
        public EnderecoController(EnderecoService service)
        {
            _service = service;
        }

        // ADICIONAR
        [HttpPost("list")]
        public async Task<IActionResult> AddRange(List<EnderecoDto> enderecoDto)
        {
            try
            {
                var addEndereco = await _service.AddRange(enderecoDto);
                
                return Created($"/api/endereco/{addEndereco.Id}", addEndereco);
            }
            catch(ArgumentException e)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"{e.Message}");
            }
        }
        // ADICIONAR
        [HttpPost]
        public async Task<IActionResult> Add(EnderecoDto enderecoDto)
        {
            try
            {
                var addEndereco = await _service.Add(enderecoDto);
                
                return Created($"/api/endereco/{addEndereco.Id}", addEndereco);
            }
            catch(ArgumentException e)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"{e.Message}");
            }
        }
        // ATUALIZAR
        [HttpPut]
        public async Task<IActionResult> Update(EnderecoDto enderecoDto)
        {
            try
            {
                var updateEndereco = await _service.Update(enderecoDto);
                
                return Ok(updateEndereco);
            }
            catch(NotFoundException e)
            {
                return this.StatusCode(StatusCodes.Status404NotFound, $"{e.Message}");
            }
            catch(ArgumentException e)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"{e.Message}");
            }
        }
        // DELETAR
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var deleteEndereco = await _service.Delete(id);
                
                return Ok(deleteEndereco);
            }
            catch(NotFoundException e)
            {
                return this.StatusCode(StatusCodes.Status404NotFound, $"{e.Message}");
            }
            catch(ArgumentException e)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"{e.Message}");
            }
        }
        // LISTAR TODOS
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var getAllEndereco = await _service.GetAll();
                
                return Ok(getAllEndereco);
            }
            catch(ArgumentException e)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"{e.Message}");
            }
        }
        // LISTAR POR ID
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var getByIdEndereco = await _service.GetById(id);
                
                return Ok(getByIdEndereco);
            }
            catch(NotFoundException e)
            {
                return this.StatusCode(StatusCodes.Status404NotFound, $"{e.Message}");
            }
            catch(ArgumentException e)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"{e.Message}");
            }
        }
        // LISTAR POR PESQUISA, CEP OU BAIRRO
        [HttpGet("buscar/{search}")]
        public async Task<IActionResult> GetByCepOrBairro(string search)
        {
            try
            {
                var getByCepOrBairroEndereco = await _service.GetBySearch(search);
                
                return Ok(getByCepOrBairroEndereco);
            }
            catch(ArgumentException e)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"{e.Message}");
            }
        }
    }
}