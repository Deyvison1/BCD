using System;

namespace BCD.Domain.Entities
{
    public class PessoasEnderecos
    {
        public DateTime Criacao { get; set; }
        public DateTime? Modificado { get; set; }
        public Pessoa Pessoa { get; set; }
        public int PessoaId { get; set; }
        public Endereco Endereco { get; set; }
        public int EnderecoId { get; set; }
    }
}