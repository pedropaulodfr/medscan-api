using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace authentication_jwt.DTO
{
    public class UserDTO
    {
        public int Id { get; set; }

        public string Username { get; set; } = null!;

        public string Email { get; set; } = null!;

        public string Password { get; set; } = null!;

        public string Role { get; set; } = null!;

        public string Token { get; set; } = null!;
        public long? UsuarioId { get; set; }
        public bool? Master { get; set; }
        public string? Nome { get; set; }
        public long? PacienteId { get; set; }
        public string? Perfil { get; set; }
    }
}