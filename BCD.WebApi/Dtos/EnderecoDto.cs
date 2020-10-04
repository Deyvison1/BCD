using System.Collections.Generic;
using BCD.WebApi.Dtos.EnumsDto;

namespace BCD.WebApi.Dtos
{
    public class EnderecoDto
    {
        public int Id { get; set; }
        public string CEP { get; set; }
        public string Logradouro { get; set; }
        public string Complemento { get; set; }
        public string Bairro { get; set; }
        public string Localidade { get; set; }
        public string UF { get; set; }
        public string Unidade { get; set; }
        public int IBGE { get; set; }
        public string GIA { get; set; }
        public List<PessoaDto> Pessoas { get; set; }
        public int IdPessoa { get; set; }
    }
}