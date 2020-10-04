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
            return solicitarContaDto;
        }
    }
}