namespace authentication_jwt.DTO
{
    public class UsuarioAutenticadoDTO
    {
        public long UsuarioId { get; set; }
        public long? PacienteId { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }
        public string Perfil { get; set; }
        public string Master { get; set; }
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
        public bool? Master { get; set; }
        public PacienteDTO? Paciente { get; set; }
    }

    public class UsuarioNotificacaoDTO
    {
        public long Notificacao_Id { get; set; }
        public DateTime Data { get; set; }
        public DateTime DataRetorno { get; set; }
        public string Medicamento { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }
        public string Email2 { get; set; }
        public string? Tipo { get; set; }
        public string? Titulo { get; set; }
        public bool? Lido { get; set; }
    }
}