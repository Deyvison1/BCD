using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace BCD.Domain.Entities.Identity
{
    public class Papel : IdentityRole<int>
    {
        public string Setor { get; set; }
        public List<UsuariosPapeis> Usuarios { get; private set; }
    }
}