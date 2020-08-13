using System.Collections.Generic;

namespace BCD.Domain.Entities
{
    public class Pessoa
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public int CPF { get; set; }
        public List<Conta> Contas { get; set; }
        public List<EnderecosPessoas> Enderecos { get; set; }
    }
}