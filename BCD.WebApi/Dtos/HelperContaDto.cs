namespace BCD.WebApi.Dtos
{
    public class HelperContaDto
    {
        public int PessoaId { get; set; }
        public float Quantia { get; set; }
        public int Agencia { get; set; }
        public int Conta { get; set; }
        public int ContaDestino { get; set; }
        public int AgenciaDestino { get; set; }
        public string Senha { get; set; }
        public string CPF { get; set; }
    }
}