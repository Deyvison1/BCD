using System;
using System.Threading.Tasks;
using BCD.WebApi.Dtos;
using BCD.WebApi.Services.Exception;
using BCD.WebApi.Services.HistoricoServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BCD.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HistoricoController : ControllerBase
    {
        private readonly HistoricoService _service;
        public HistoricoController(HistoricoService service)
        {
            _service = service;
        }
        // LISTAR ULTIMOS MESES
        [HttpGet("listLast/{agencia}/{conta}/{tipoConta}")]
        public async Task<IActionResult> GetLastMeses(int agencia, int conta, int tipoConta) 
        {
            var historicoByMes = await _service.GetLastMeses(agencia, conta, tipoConta);

            return Ok(historicoByMes); 
        }
        // LISTAR PELO MES
        [HttpGet("listarPeloMesCorrente/{mes}/{agencia}/{conta}")]
        public async Task<IActionResult> GetByMesCorrente(int mes, int agencia, int conta) 
        {
            var historicoByMes = await _service.GetByMesCorrente(mes, agencia, conta);

            return Ok(historicoByMes); 
        }
        // LISTAR PELO MES CORRENTE
        [HttpGet("listarPeloMesPoupanca/{mes}/{agencia}/{conta}")]
        public async Task<IActionResult> GetByMesPoupanca(int mes, int agencia, int conta) 
        {
            var historicoByMes = await _service.GeteByMesPoupanca(mes, agencia, conta);

            return Ok(historicoByMes);    
        }
        // ADICIONAR
        [HttpPost]
        public async Task<IActionResult> Add(HistoricoDto historicoDto) 
        {
            try 
            {
                var addHistorico = await _service.Add(historicoDto);

                return Created($"/api/historico/{addHistorico.Id}", addHistorico);
            }
            catch(ArgumentException e)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"${e.Message}");
            }
        }
        // ATUALIZAR
        [HttpPut]
        public async Task<IActionResult> Update(HistoricoDto historicoDto) 
        {
            try 
            {
                var updateHistorico = await _service.Update(historicoDto);

                return Ok(updateHistorico);
            }
            catch(NotFoundException e)
            {
                return this.StatusCode(StatusCodes.Status404NotFound, $"${e.Message}");
            }
            catch(ArgumentException e)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"${e.Message}");
            }
        }
        // DELETAR
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id) 
        {
            try 
            {
                var deleteHistorico = await _service.Delete(id);

                return Ok(deleteHistorico);
            }
            catch(NotFoundException e)
            {
                return this.StatusCode(StatusCodes.Status404NotFound, $"${e.Message}");
            }
            catch(ArgumentException e)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"${e.Message}");
            }
        }
        // LISTAR TODOS
        [HttpGet]
        public async Task<IActionResult> GetAll() 
        {
            try 
            {
                var getAllHistorico = await _service.GetAll();

                return Ok(getAllHistorico);
            }
            catch(ArgumentException e)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"${e.Message}");
            }
        }
        // LISTAR POR ID
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id) 
        {
            try 
            {
                var getByIdHistorico = await _service.GetById(id);

                return Ok(getByIdHistorico);
            }
            catch(ArgumentException e)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"${e.Message}");
            }
        }
        // LISTAR POR PESQUISA, VALOR OU DESCRICAO OU CONTA
        [HttpGet("buscar/{search}")]
        public async Task<IActionResult> GetBySearch(string search) 
        {
            try 
            {
                var getByIdHistorico = await _service.GetValorOrDescricaoOrConta(search);

                return Ok(getByIdHistorico);
            }
            catch(ArgumentException e)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"${e.Message}");
            }
        }
    }
}