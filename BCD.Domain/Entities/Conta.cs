using System.Collections.Generic;
using BCD.Domain.Entities.Enums;

namespace BCD.Domain.Entities
{
    public class Conta
    {
        public int Id { get; set; }
        public int DigitosConta { get; private set; }
        public int DigitosAgencia { get; private set; }
        public EnumTipoConta TipoConta { get; private set; }
        public string NomeConta { get; set; }
        public float Saldo { get; set; }
        public string CPF { get; private set; }
        public string Senha { get; private set; }
        public int PessoaId { get; private set; }
        public Pessoa Pessoa { get; set; }
        public List<HistoricosContas> Extrato { get; private set; }
    }
}