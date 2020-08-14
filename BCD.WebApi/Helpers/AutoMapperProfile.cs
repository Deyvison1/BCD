using System.Linq;
using AutoMapper;
using BCD.Domain.Entities;
using BCD.Domain.Entities.Enums;
using BCD.WebApi.Dtos;
using BCD.WebApi.Dtos.EnumsDto;

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
            CreateMap<Endereco, EnderecoDto>().ReverseMap();
            //
            CreateMap<Historico, HistoricoDto>().ReverseMap();
            CreateMap<Conta, ContaDto>().ForMember(
                x => x.Extrato, opt => {
                    opt.MapFrom(src => src.Extrato.Select(
                        x => x.Historico
                    ).ToList());
                }
            ).ReverseMap();
            CreateMap<EnumUF, EnumUFDto>().ReverseMap();
            CreateMap<EnumTipoContaDto, EnumTipoContaDto>().ReverseMap();
        }
    }
}