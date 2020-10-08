using System.Collections.Generic;
using BCD.Domain.Entities.Enums;

namespace BCD.Domain.Entities
{
    public class Pessoa
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string CPF { get; set; }
        public EnumEstado Situacao { get; set; }
        public List<Conta> Contas { get; set; }
        public int EnderecoId { get; set; }
        public Endereco Endereco { get; set; }
    }
}