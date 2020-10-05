using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using BCD.WebApi.Dtos.EnumsDto;

namespace BCD.WebApi.Dtos
{
    public class SolicitarContaDto
    {
        public string NomeConta { get; set; }
        public string CPF { get; set; }
        public int TipoConta { get; set; }
        public string Senha { get; set; }
        public List<EnderecoDto> Enderecos { get; set; }

    }
}