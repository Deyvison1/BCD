using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BCD.WebApi.Dtos;
using BCD.WebApi.Services.EnderecoServices;
using BCD.WebApi.Services.PessoaServices;

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

        public async Task<SolicitarContaDto> Add(SolicitarContaDto solicitarContaDto) 
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

            List<EnderecoDto> enderecosDto = new List<EnderecoDto>();
            
            enderecosDto.AddRange(solicitarContaDto.Enderecos);
            
            enderecosDto.ForEach(
                x => 
                    x.PessoaId = pessoaAdicionada.Id
            );
            
            var enderecosAdicionados = await _enderecoService.AddRange(enderecosDto);
            
            return solicitarContaDto;
        }
    }
}