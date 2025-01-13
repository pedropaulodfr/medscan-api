using System;
using System.Collections.Generic;

namespace authentication_jwt.Models;

public partial class Usuario
{
    public long Id { get; set; }

    public string Perfil { get; set; } = null!;

    public string Nome { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Senha { get; set; } = null!;

    public string? ImagemPerfil { get; set; }

    public string? CodigoCadastro { get; set; }

    public bool Ativo { get; set; }

    public virtual ICollection<Paciente> Pacientes { get; } = new List<Paciente>();
}
