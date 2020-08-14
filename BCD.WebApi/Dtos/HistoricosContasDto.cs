using System;

namespace BCD.WebApi.Dtos
{
    public class HistoricosContasDto
    {
        public DateTime DataCriacao { get; set; }
        public int HistoricoId { get; set; }
        public HistoricoDto Historico { get; set; }
        public int ContaId { get; set; }
        public ContaDto Conta { get; set; }
    }
}