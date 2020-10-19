using System.ComponentModel.DataAnnotations;

namespace BCD.WebApi.Dtos
{
    public class UsuarioLoginDto
    {
        [Required(ErrorMessage = "{0} é obrigatorio")]
        [EmailAddress]
        public string Email { get; set; }
        [Required(ErrorMessage = "{0} é obrigatorio")]
        public string Password { get; set; }
        
    }
}