using System;
using System.Threading.Tasks;
using BCD.WebApi.Dtos;
using BCD.WebApi.Services.ContaServices;
using BCD.WebApi.Services.Exception;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BCD.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContaController : ControllerBase
    {
        private readonly ContaService _service;

        public ContaController(ContaService service)
        {
            _service = service;
        }
        // REALIZAR SAQUE
        [HttpPut("saque")]
        public async Task<IActionResult> Saque(HelperContaDto contaSaque)
        {
            try
            {
                var saqueConta = await _service.Saque(contaSaque);

                return Ok(saqueConta);
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
        // ADICIONAR
        [HttpPost]
        public async Task<IActionResult> Add(ContaDto contaDto) {
            try {
                var contaAdd = await _service.Add(contaDto);

                return Created($"/api/conta/{contaAdd.Id}", contaAdd);
            }catch(ArgumentException e) {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"{e.Message}");
            }
        }
        // ATUALIZAR
        [HttpPut]
        public async Task<IActionResult> Update(ContaDto contaDto) {
            try {
                var contaUpdate = await _service.Update(contaDto);

                return Ok(contaUpdate);
            }
            catch(NotFoundException e) {
                return this.StatusCode(StatusCodes.Status404NotFound, $"{e.Message}");
            }
            catch(ArgumentException e) {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"{e.Message}");
            }
        }
        // DELETAR
        [HttpDelete]
        public async Task<IActionResult> Delete(int id) {
            try {
                var contaDelete = await _service.Delete(id);

                return Ok(contaDelete);
            }
            catch(NotFoundException e) {
                return this.StatusCode(StatusCodes.Status404NotFound, $"{e.Message}");
            }
            catch(ArgumentException e) {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"{e.Message}");
            }
        }
        // LISTAR TODOS
        [HttpGet]
        public async Task<IActionResult> GetAll() {
            try {
                var getAllConta = await _service.GetAll();

                return Ok(getAllConta);
            }
            catch(ArgumentException e) {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"{e.Message}");
            }
        }
        // LISTAR POR ID
        [HttpGet("{id}")]
        public async Task<IActionResult> GetAll(int id) {
            try {
                var getByIdConta = await _service.GetById(id);

                return Ok(getByIdConta);
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
                var getBySearchConta = await _service.GetBySearch(search);

                return Ok(getBySearchConta);
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