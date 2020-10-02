using System.Collections.Generic;
using BCD.Domain.Entities.Enums;

namespace BCD.Domain.Entities
{
    public class Endereco
    {
        public int Id { get; private set; }
        public int CEP { get; set; }
        public string Logradouro { get; set; }
        public string Complemento { get; set; }
        public string Bairro { get; set; }
        public string Localidade { get; set; }
        public EnumUF UF { get; set; }
        public string Unidade { get; private set; }
        public int IBGE { get; private set; }
        public string GIA { get; private set; }
        public List<EnderecosPessoas> Pessoa { get; private set; }
    }
}