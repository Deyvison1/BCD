using System;
using System.Threading.Tasks;
using BCD.WebApi.Dtos;
using BCD.WebApi.Services.Exception;
using BCD.WebApi.Services.PessoaServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BCD.WebApi.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class PessoaController : ControllerBase
    {
        private readonly PessoaService _service;
        public PessoaController(PessoaService service)
        {
            _service = service;
        }
        // ADICIONAR
        [HttpPost]
        public async Task<IActionResult> Add(PessoaDto pessoaDto) {
            try {
                var pessoaAdd = await _service.Add(pessoaDto);

                return Created($"/api/pessoa/{pessoaAdd.Id}", pessoaAdd);
            }catch(ArgumentException e) {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"{e.Message}");
            }
        }
        // ATUALIZAR
        [HttpPut]
        public async Task<IActionResult> Update(PessoaDto pessoaDto) {
            try {
                var pessoaUpdate = await _service.Update(pessoaDto);

                return Ok(pessoaUpdate);
            }
            catch(NotFoundException e) {
                return this.StatusCode(StatusCodes.Status404NotFound, $"{e.Message}");
            }
            catch(ArgumentException e) {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"{e.Message}");
            }
        }
        // DELETAR
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id) {
            try {
                var pessoaDelete = await _service.Delete(id);

                return Ok(pessoaDelete);
            }
            catch(NotFoundException e) {
                return this.StatusCode(StatusCodes.Status404NotFound, $"{e.Message}");
            }
            catch(ArgumentException e) {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"{e.Message}");
            }
        }
        // LISTAR POR ID PESSOA 
        [HttpGet("byId/{idPessoa}")]
        public async Task<IActionResult> GetAllByIdPessoaAsync(int idPessoa) {
            try {
                var getAllPessoa = await _service.GetAllByIdPessoaasync(idPessoa);

                return Ok(getAllPessoa);
            }
            catch(ArgumentException e) {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"{e.Message}");
            }
        }
        // LISTAR POR ID PESSOA LISTA
        [HttpGet("details/{idPessoa}")]
        public async Task<IActionResult> GetAllByIdPessoa(int idPessoa) {
            try {
                var getAllPessoa = await _service.GetAllByIdPessoa(idPessoa);

                return Ok(getAllPessoa);
            }
            catch(ArgumentException e) {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"{e.Message}");
            }
        }
        // LISTAR TODOS
        [HttpGet]
        public async Task<IActionResult> GetAll() {
            try {
                var getAllPessoa = await _service.GetAll();

                return Ok(getAllPessoa);
            }
            catch(ArgumentException e) {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"{e.Message}");
            }
        }
        // LISTAR POR ID
        [HttpGet("{id}")]
        public async Task<IActionResult> GetAll(int id) {
            try {
                var getByIdPessoa = await _service.GetById(id);

                return Ok(getByIdPessoa);
            }
            catch(NotFoundException e) {
                return this.StatusCode(StatusCodes.Status404NotFound, $"{e.Message}");
            }
            catch(ArgumentException e) {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"{e.Message}");
            }
        }
        // LISTAR PESQUISA, NOME OU CPF
        [HttpGet("buscar/{search}")]
        public async Task<IActionResult> GetBySearch(string search) {
            try {
                var getBySearchPessoa = await _service.GetBySearch(search);

                return Ok(getBySearchPessoa);
            }
            catch(NotFoundException e) {
                return this.StatusCode(StatusCodes.Status404NotFound, $"{e.Message}");
            }
            catch(ArgumentException e) {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"{e.Message}");
            }
        }
    }
}