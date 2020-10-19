using BCD.Domain.Entities;
using BCD.Domain.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BCD.Repository.Data
{
    public class BCDContext : IdentityDbContext<Usuario, Papel, int,
        IdentityUserClaim<int>, UsuariosPapeis, IdentityUserLogin<int>, IdentityRoleClaim<int>,
        IdentityUserToken<int>>
    {
        public BCDContext(DbContextOptions<BCDContext> options): base(options) {  }

        public DbSet<Pessoa> Pessoas { get; set; }
        public DbSet<Conta> Contas { get; set; }
        public DbSet<Historico> Historicos { get; set; }
        public DbSet<HistoricosContas> HistoricosContas { get; set; }
        public DbSet<Endereco> Enderecos { get; set; }
        public DbSet<ContaCadastrada> ContasCadastradas { get; set; }

        protected override void OnModelCreating(ModelBuilder builder) 
        {
            base.OnModelCreating(builder);

            builder.Entity<UsuariosPapeis>(usuarioPapel => 
            {
                usuarioPapel.HasKey(up => new { up.UserId, up.RoleId });

                usuarioPapel.HasOne(up => up.Papel)
                    .WithMany(p => p.Usuarios)
                    .HasForeignKey(up => up.RoleId)
                    .IsRequired();

                usuarioPapel.HasOne(up => up.Usuario)
                    .WithMany(u => u.Papeis)
                    .HasForeignKey(up => up.UserId)
                    .IsRequired();
            });

            builder.Entity<HistoricosContas>().HasKey(hc => new { hc.HistoricoId , hc.ContaId });
        }
    }
}