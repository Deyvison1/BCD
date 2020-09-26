namespace BCD.Domain.Entities
{
    public class ContaCadastrada
    {
        public int Id { get; private set; }
        public Conta Conta { get; private set; }
        public int ContaId { get; private set; }
        public Pessoa Pessoa { get; private set; }
        public int PessoaId { get; private set; }
    }
}