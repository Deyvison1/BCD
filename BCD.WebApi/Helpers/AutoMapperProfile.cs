using System.Linq;
using AutoMapper;
using BCD.Domain.Entities;
using BCD.WebApi.Dtos;

namespace BCD.WebApi.Helpers
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            //
            CreateMap<Pessoa, PessoaDto>().ForMember(
                x => x.Enderecos, opt => {
                    opt.MapFrom(src => src.Enderecos.Select(
                        x => x.Endereco
                    ).ToList());
                }
            ).ReverseMap();
            //
            CreateMap<Endereco, EnderecoDto>().ForMember(
                x => x.Pessoas, opt => {
                    opt.MapFrom(src => src.Pessoas.Select(
                        x => x.Pessoa
                    ).ToList());
                }
            ).ReverseMap();
            //
            CreateMap<Historico, HistoricoDto>().ForMember(
                x => x.Contas, opt => {
                    opt.MapFrom(src => src.Contas.Select(
                        x => x.Conta
                    ).ToList());
                }
            ).ReverseMap();
            CreateMap<Conta, ContaDto>().ForMember(
                x => x.Extrato, opt => {
                    opt.MapFrom(src => src.Extrato.Select(
                        x => x.Historico
                    ).ToList());
                }
            ).ReverseMap();
        }
    }
}