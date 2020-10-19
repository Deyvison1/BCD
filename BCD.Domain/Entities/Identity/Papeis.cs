using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace BCD.Domain.Entities.Identity
{
    public class Papel : IdentityRole<int>
    {
        public List<UsuariosPapeis> Usuarios { get; set; }
    }
}