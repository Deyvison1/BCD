namespace BCD.WebApi.Dtos
{
    public class UsuarioDto
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string NomeCompleto { get; set; }
        public string Email { get; set; }
        public string Setor { get; set; }
        public string Papel { get; set; }
        public int PessoaId { get; set; }
        public PessoaDto Pessoa { get; set; }
        public string Password { get; set; }
        public string NormalizedUserName { get; set; }
    }
}