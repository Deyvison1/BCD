using BCD.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace BCD.Repository.Data
{
    public class BCDContext : DbContext
    {
        public BCDContext(DbContextOptions<BCDContext> options): base(options) {  }

        public DbSet<Pessoa> Pessoas { get; set; }
        public DbSet<Conta> Contas { get; set; }
        public DbSet<Historico> Historicos { get; set; }
        public DbSet<HistoricosContas> HistoricosContas { get; set; }
        public DbSet<Endereco> Enderecos { get; set; }
        public DbSet<ContaCadastrada> ContasCadastradas { get; set; }
        public DbSet<PessoasEnderecos> PessoasEnderecos { get; set; }

        protected override void OnModelCreating(ModelBuilder builder) 
        {
            base.OnModelCreating(builder);

            builder.Entity<PessoasEnderecos>().HasKey(pe => new { pe.PessoaId, pe.EnderecoId });
            builder.Entity<HistoricosContas>().HasKey(hc => new { hc.HistoricoId , hc.ContaId });
        }
    }
}