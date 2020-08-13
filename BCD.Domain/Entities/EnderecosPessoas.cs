using System;

namespace BCD.Domain.Entities
{
    public class EnderecosPessoas
    {
        public DateTime DataAtualizacao { get; private set; }
        public int EnderecoId { get; private set; }
        public Endereco Endereco { get; set; }
        public Pessoa Pessoa { get; set; }
        public int PessoaId { get; private set; }
    }
}