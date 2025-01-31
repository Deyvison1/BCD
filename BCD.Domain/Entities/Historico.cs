using System;
using System.Collections.Generic;
using BCD.Domain.Entities.Enums;

namespace BCD.Domain.Entities
{
    public class Historico
    {
        public int Id { get; private set; }
        public DateTime DataTransacao { get; private set; }
        public float Valor { get; private set; }
        public string DescricaoTransacao { get; private set; }
        public EnumTipoConta TipoConta { get; set; }
        public string NomeContaDestino { get; private set; }
        public int Operacao { get; private set; }
        public string NomeConta { get; private set; }
        public int DigitosConta { get; private set; }
        public int DigitosAgencia { get; private set; }
        public int DigitosAgenciaDestino { get; private set; }
        public int DigitosContaDestino { get; private set; }
        public List<HistoricosContas> Contas { get; private set; }
    }
}