using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace BCD.Domain.Entities.Identity
{
    public class Usuario : IdentityUser<int>
    {
        [Column(TypeName = "nvarchar(150)")]
        public string NomeCompleto { get; private set; }
        public int PessoaId { get; private set; }
        public Pessoa Pessoa { get; private set; }
        public List<UsuariosPapeis> Papeis { get; private set; }
    }
}