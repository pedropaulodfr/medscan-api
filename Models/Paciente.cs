using System;
using System.Collections.Generic;

namespace authentication_jwt.Models;

public partial class Paciente
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

    public string? Uf { get; set; }

    public string? Cep { get; set; }

    public string? Cns { get; set; }

    public string? PlanoSaude { get; set; }

    public long UsuariosId { get; set; }

    public string? Cidade { get; set; }

    public bool? Deletado { get; set; }

    public virtual ICollection<CartaoControle> CartaoControles { get; } = new List<CartaoControle>();

    public virtual Usuario Usuarios { get; set; } = null!;
}
