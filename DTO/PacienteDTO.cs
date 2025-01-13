using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using authentication_jwt.Models;

namespace authentication_jwt.DTO
{
    public class PacienteDTO
    {
        public long Id { get; set; }

        public string? Nome { get; set; }

        public string? NomeCompleto { get; set; }

        public string? Email { get; set; }

        public string Cpf { get; set; } = null!;

        public DateTime? DataNascimento { get; set; }

        public string? Logradouro { get; set; }

        public string? Bairro { get; set; }

        public string? Complemento { get; set; }

        public string? Numero { get; set; }

        public string? Cidade { get; set; }
        public string? Uf { get; set; }

        public string? Cep { get; set; }
        public string? Endereco { get; set; }

        public string? Cns { get; set; }

        public string? PlanoSaude { get; set; }

        public long UsuariosId { get; set; }

        public UsuarioDTO? Usuarios { get; set; } = null!;
    }
}