using System;
using System.Collections.Generic;
using BCD.WebApi.Dtos.EnumsDto;

namespace BCD.WebApi.Dtos
{
    public class HistoricoDto
    {
        public int Id { get; set; }
        public DateTime DataTransacao { get; set; }
        public float Valor { get; set; }
        public string DescricaoTransacao { get; set; }
        public string TipoConta { get; set; }
        public string NomeConta { get; set; }
        public int DigitosConta { get; set; }
        public int DigitosAgencia { get; set; }
        public int DigitosAgenciaDestino { get; set; }
        public int DigitosContaDestino { get; set; }
    }
}