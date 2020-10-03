using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using BCD.WebApi.Dtos.EnumsDto;

namespace BCD.WebApi.Dtos
{
    public class SolicitarContaDto
    {
        [Required(ErrorMessage = "{0} é obrigatorio")]
        public string NomeConta { get; set; }
        [Required(ErrorMessage = "{0} é obrigatorio")]
        [StringLength(11, ErrorMessage = "{0} deve ter no maxino 11 caracteres")]
        public string CPF { get; set; }
        [Required(ErrorMessage = "{0} é obrigatorio")]
        public EnumTipoContaDto TipoConta { get; set; }
        [Required(ErrorMessage = "{0} é obrigatorio")]
        public string Senha { get; set; }
        public List<EnderecoDto> Enderecos { get; set; }

    }
}