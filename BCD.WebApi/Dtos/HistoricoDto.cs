using System.Collections.Generic;
using BCD.WebApi.Dtos.EnumsDto;

namespace BCD.WebApi.Dtos
{
    public class HistoricoDto
    {
        public int Id { get; set; }
        public float Valor { get; set; }
        public string DescricaoTransacao { get; set; }
        public EnumTipoContaDto TipoConta { get; set; }
        public int DigitosConta { get; set; }
        public int DigitosAgencia { get; set; }
        public int DigitosAgenciaDestino { get; set; }
        public int DigitosContaDestino { get; set; }
        public int ContaId { get; set; }
        public ContaDto Conta { get; set; }
        public List<ContaDto> Contas { get; set; }
    }
}