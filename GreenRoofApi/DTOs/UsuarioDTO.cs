namespace GreenRoofApi.DTOs
{
    public class UsuarioDTO
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
        public string Senha { get; internal set; }
    }

    public class UsuarioLoginDTO
    {
        public string Email { get; set; }
        public string Senha { get; set; }
    }

    public class UsuarioRegisterDTO
    {
        public string Nome { get; set; } = "Luis Marcello";
        public string Email { get; set; } = "luis.marcello@example.com";
        public string Senha { get; set; } = "minhasenha";
        public string Role { get; set; } = "Cliente"; // Definindo o role padrão como Cliente
    }

    public class UsuarioResultDTO
    {
        public bool Succeeded { get; set; }
        public string Token { get; set; }
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
        public IEnumerable<string> Errors { get; set; }
    }
}
