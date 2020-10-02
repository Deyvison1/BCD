using System;
using System.Threading.Tasks;
using BCD.WebApi.Dtos;
using BCD.WebApi.Services.ContaCadastradaServices;
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
        // LISTAR CONTAS CADASTRADAS
        [HttpGet("contaCadastrada/{pessoaId}")]
        public async Task<IActionResult> GetContaCadastrada(int pessoaId)
        {
            try
            {
                var contaCadastrada = await _service.GetAllContaCadastradaByPessoaId(pessoaId);

                return Ok(contaCadastrada);
            } 
            catch(ArgumentException e) 
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"{e.Message}");
            }
        }
        // PEGAR MES ATUAL
        [HttpGet("mesAtual")]
        public IActionResult PegarMesAtual()
        {
            int mesAtual = _service.PegarMesAtual();
            
            return Ok(mesAtual);
        }
        // APLICAR VALOR POUPANCA
        [HttpPut("aplicarPoupanca")]
        public async Task<IActionResult> AplicarPoupanca(HelperContaDto contaResgatar)
        {
            try
            {
                var conta = await _service.AplicarPoupanca(contaResgatar);

                return Ok(conta);
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
        // RESGATAR VALOR POUPANCA
        [HttpPut("resgatarPoupanca")]
        public async Task<IActionResult> ResgatarPoupanca(HelperContaDto contaResgatar)
        {
            try
            {
                var conta = await _service.ResgatarPoupanca(contaResgatar);

                return Ok(conta);
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
        // REALIZAR TRANSFERENCIA
        [HttpPut("transferencia")]
        public async Task<IActionResult> Transferencia(HelperContaDto contaTransferencia)
        {
            try
            {
                var conta = await _service.Transferencia(contaTransferencia);

                return Ok(conta);
            }
            catch(NotFoundException e)
            {
                return this.StatusCode(StatusCodes.Status404NotFound, $"{e.Message}");
            }
            catch (ArgumentException e)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"{e.Message}");
            }
        }
        // REALIZAR DEPOSITO EM CONTA CORRENTE
        [HttpPut("deposito")]
        public async Task<IActionResult> Depositar(HelperContaDto contaDeposito)
        {
            try
            {
                var conta = await _service.Depositar(contaDeposito);

                return Ok(conta);
            }
            catch(NotFoundException e)
            {
                return this.StatusCode(StatusCodes.Status404NotFound, $"{e.Message}");
            }
            catch (ArgumentException e)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"{e.Message}");
            }
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
        // SOLICITAR CONTA
         [HttpPost("solicitar")]
        public async Task<IActionResult> SolicitarConta(SolicitarConta contaDto) {
            try {
                var contaRequest = await _service.SolicitarConta(contaDto);

                return Ok(contaRequest);
            }
            catch(NotFoundException e) {
                return this.StatusCode(StatusCodes.Status404NotFound, $"{e.Message}");
            }
            catch(ArgumentException e) {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"{e.Message}");
            }
        }
        // ADICIONAR
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
        [HttpDelete("{id}")]
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
        // LISTAR POR AGENCIA E CONTA
        [HttpGet("contaAndAgencia/{conta}/{agencia}")]
        public async Task<IActionResult> GetByAgenciaAndConta(int conta, int agencia) {
            try 
            {
                var contaByAgenciaAndConta = await _service.GetByContaAndAgencia(conta, agencia);

                return Ok(contaByAgenciaAndConta);
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
        // LISTAR POR ID COM EXTRATO
        [HttpGet("listarPorIdPessoa/{idPessoa}")]
        public async Task<IActionResult> GetByIdLista(int idPessoa) {
            try {
                var conta = await _service.ListGetByIdPessoa(idPessoa);
                return Ok(conta);
            }
            catch(NotFoundException e) {
                return this.StatusCode(StatusCodes.Status404NotFound, $"{e.Message}");
            }
            catch(ArgumentException e) {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"{e.Message}");
            }
        }
        [HttpGet("nomeConta/{nomeConta}")]
        public async Task<IActionResult> GetAllDeleteNomeCurrency(string nomeConta) {
            try {
                var contas = await _service.GetAllDeleteNomeCurrency(nomeConta);

                return Ok(contas);
            }catch(ArgumentException e) {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"{e}");
            }
        }
    }
}