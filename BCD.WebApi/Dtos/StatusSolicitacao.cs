using System.ComponentModel.DataAnnotations;

namespace BCD.WebApi.Dtos
{
    public class StatusSolicitacao
    {
        [Required(ErrorMessage = "{0} é obrigatorio")]
        public string CPF { get; set; }
        [Required(ErrorMessage = "{0} é obrigatorio")]
        public string Senha { get; set; }
    }
}