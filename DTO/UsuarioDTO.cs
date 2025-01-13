namespace authentication_jwt.DTO
{
    public class UsuarioAutenticadoDTO
    {
        public long Usuario_Id { get; set; }
        public long? Paciente_Id { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }
        public string Perfil { get; set; }
        public string Token { get; set; }
    }

    public class UsuarioDTO
    {
        public long? Id { get; set; }

        public string? Perfil { get; set; }

        public string? Nome { get; set; }

        public string? Email { get; set; }

        public string? ImagemPerfil { get; set; }

        public string? CodigoCadastro { get; set; }

        public string? Ativo { get; set; }
        public string? Senha { get; set; }
        public string? Token { get; set; }
        public long? PacienteId { get; set; }
        public PacienteDTO? Paciente { get; set; }
    }
}