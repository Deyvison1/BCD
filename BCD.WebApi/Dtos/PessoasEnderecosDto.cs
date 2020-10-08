using System;

namespace BCD.WebApi.Dtos
{
    public class PessoasEnderecosDto
    {
        public DateTime Criacao { get; set; }
        public DateTime? Modificado { get; set; }
        public PessoaDto Pessoa { get; set; }
        public int PessoaId { get; set; }
        public EnderecoDto Endereco { get; set; }
        public int EnderecoId { get; set; }
    }
}