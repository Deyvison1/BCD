using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using BCD.WebApi.Dtos.EnumsDto;

namespace BCD.WebApi.Dtos
{
    public class ContaDto
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "{0} é obrigatorio!")]
        public int DigitosConta { get; set; }
        [Required(ErrorMessage = "{0} é obrigatorio!")]
        public int DigitosAgencia { get; set; }
        [Required(ErrorMessage = "{0} é obrigatorio!")]
        [Range(0, 1)]
        public EnumTipoContaDto TipoConta { get; set; }
        [Required(ErrorMessage = "{0} é obrigatorio!")]
        [StringLength(50, ErrorMessage = "{0} deve ter no maximo 50 caracteres!")]
        public string NomeConta { get; set; }
        public float Saldo { get; set; }
        [Required(ErrorMessage = "{0} é obrigatorio!")]
        public int PessoaId { get; set; }
        [Required(ErrorMessage = "{0} é obrigatório!")]
        public string Senha { get; set; }

        public List<HistoricoDto> Extrato { get; set; }
        public float ValorTotal { get; set; }
    }
}