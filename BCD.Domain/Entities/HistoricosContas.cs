using System;

namespace BCD.Domain.Entities
{
    public class HistoricosContas
    {
        public DateTime DataCriacao { get; set; }
        public int HistoricoId { get; set; }
        public Historico Historico { get; set; }
        public int ContaId { get; set; }
        public Conta Conta { get; set; }
    }
}