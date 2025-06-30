using System;
using System.Collections.Generic;

namespace authentication_jwt.Models;

public partial class Log
{
    public long Id { get; set; }

    public DateTime DataHora { get; set; }

    public long? UsuarioId { get; set; }

    public string? Acao { get; set; }

    public string? JsonAntigo { get; set; }

    public string? JsonNovo { get; set; }

    public virtual Usuario? Usuario { get; set; }
}
