using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BCD.WebApi.Dtos
{
    public class PessoaDto
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "{0} é obrigatorio!")]
        [StringLength(100, ErrorMessage = "{0} deve ter no maximo 100 caracteres!")]
        public string Nome { get; set; }
        [Required(ErrorMessage = "{0} é obrigatorio!")]
        public string CPF { get; set; }
        public List<ContaDto> Contas { get; set; }
        public List<EnderecoDto> Enderecos { get; set; }
    }
}