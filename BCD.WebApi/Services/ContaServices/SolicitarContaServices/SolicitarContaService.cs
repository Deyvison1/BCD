using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using BCD.WebApi.Dtos;
using BCD.WebApi.Services.EnderecoServices;
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

        public SolicitarContaService(IMapper mapper, EnderecoService enderecoService, PessoaService pessoaService, ContaService contaService)
        {
            _contaService = contaService;
            _pessoaService = pessoaService; 
            _enderecoService = enderecoService; 
            _map = mapper;
        }

        public async Task<EnderecoDto[]> Add(SolicitarContaDto solicitarContaDto) 
        {
            // MONTANDO O OBJETO DO TIPO PESSOA
            PessoaDto pessoaDto = new PessoaDto 
            {
                Nome = solicitarContaDto.NomeConta,
                Situacao = 3,
                CPF = solicitarContaDto.CPF
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
            
            var enderecos = await GetEnderecoByCep(solicitarContaDto.Ceps);
            
            enderecos.ForEach(
                x => x.PessoaId = pessoaAdicionada.Id
            );
            var enderecosAdicionados = await _enderecoService.AddRange(enderecos);
            
            return enderecosAdicionados;
        }

        public async Task<List<EnderecoDto>> GetEnderecoByCep(List<Cep> ceps)
        {
            List<EnderecoDto> enderecos = new List<EnderecoDto>();
            foreach(Cep cepFor in ceps)
            {
            var requisicaoWeb = WebRequest.CreateHttp("https://viacep.com.br/ws/"+cepFor.cep+"/json/");
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