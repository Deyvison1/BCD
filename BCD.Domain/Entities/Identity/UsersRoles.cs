using Microsoft.AspNetCore.Identity;

namespace BCD.Domain.Entities.Identity
{
    public class UsuariosPapeis : IdentityUserRole<int>
    {
        public Usuario Usuario { get; set; }
        public Papel Papel { get; set; }
    }
}