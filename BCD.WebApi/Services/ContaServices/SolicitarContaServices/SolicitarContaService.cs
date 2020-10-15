using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using BCD.WebApi.Dtos;
using BCD.WebApi.Services.EnderecoServices;
using BCD.WebApi.Services.Exception;
using BCD.WebApi.Services.PessoaServices;
using Newtonsoft.Json;

namespace BCD.WebApi.Services.ContaServices.SolicitarContaServices
{
    public class SolicitarContaService
    {
        public IMapper _map { get; }
        public EnderecoService _enderecoService { get; }
        public PessoaService _pessoaService { get; }
        public ContaService _contaService { get; }

        public SolicitarContaService(IMapper mapper, EnderecoService enderecoService,
            PessoaService pessoaService, ContaService contaService
            )
        {
            _contaService = contaService;
            _pessoaService = pessoaService;
            _enderecoService = enderecoService;
            _map = mapper;
        }
        // MOSTRAR ESTADO DA SOLICITACAO
        public async Task<int> StatusSolicitacao(StatusSolicitacao status)
        {
            var situacao = await _contaService.GetStatusBySolicitacao(status.CPF);
            if (situacao == null)
            {
                throw new NotFoundException("CPF Incorreto");
            }

            bool compareSenha = _contaService.CompareSenha(status.Senha, situacao.Senha);
            if (compareSenha)
            {
                return situacao.Situacao;
            }
            throw new ArgumentException("Senha Incorreta");
        }
        // SOLICITAR CONTA
        public async Task<EnderecoDto> Add(SolicitarContaDto solicitarContaDto)
        {
            var enderecoAdicionado = await _enderecoService.Add(solicitarContaDto.Endereco);
            // MONTANDO O OBJETO DO TIPO PESSOA
            PessoaDto pessoaDto = new PessoaDto
            {
                Nome = solicitarContaDto.NomeConta,
                CPF = solicitarContaDto.CPF,
                EnderecoId = enderecoAdicionado.Id
            };
            // INSERINDO NO BANCO
            var pessoaAdicionada = await _pessoaService.Add(pessoaDto);

            // MONTANDO O OBJETO DO TIPO CONTA
            ContaDto contaDto = new ContaDto
            {
                NomeConta = solicitarContaDto.NomeConta,
                CPF = solicitarContaDto.CPF,
                Senha = solicitarContaDto.Senha,
                TipoConta = solicitarContaDto.TipoConta,
                Situacao = 3,
                PessoaId = pessoaAdicionada.Id
            };
            // INSERINDO NO BANCO 
            var contaAdicionada = await _contaService.Add(contaDto);

            return enderecoAdicionado;
        }
        // BUSCAR ENDERECO PELO CEP
        public async Task<List<EnderecoDto>> GetEnderecoByCep(List<Cep> ceps)
        {
            List<EnderecoDto> enderecos = new List<EnderecoDto>();
            foreach (Cep cepFor in ceps)
            {
                var requisicaoWeb = WebRequest.CreateHttp("https://viacep.com.br/ws/" + cepFor.cep + "/json/");
                requisicaoWeb.Method = "GET";
                requisicaoWeb.UserAgent = "RequisicaoWebDemo";
                using (var resposta = requisicaoWeb.GetResponse())
                {
                    var streamDados = resposta.GetResponseStream();
                    StreamReader reader = new StreamReader(streamDados);
                    object objResponse = await reader.ReadToEndAsync();
                    var endereco = JsonConvert.DeserializeObject<EnderecoDto>(objResponse.ToString());
                    enderecos.Add(endereco);
                    streamDados.Close();
                    resposta.Close();
                }
            }
            return enderecos;
        }


    }
}