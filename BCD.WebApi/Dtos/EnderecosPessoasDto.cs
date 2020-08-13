using System;
namespace BCD.WebApi.Dtos
{
    public class EnderecosPessoasDto
    {
        public DateTime DataAtualizacao { get; set; }
        public int EnderecoId { get; set; }
        public EnderecoDto Endereco { get; set; }
        public PessoaDto Pessoa { get; set; }
        public int PessoaId { get; set; }
    }
}
