using System;
using System.Collections.Generic;

namespace authentication_jwt.Models;

public partial class Notificaco
{
    public long Id { get; set; }

    public DateTime? Data { get; set; }

    public string? Tipo { get; set; }

    public long? CartaoControleId { get; set; }

    public long EmailId { get; set; }

    public long? UsuarioId { get; set; }

    public long? PacienteId { get; set; }

    public bool? Enviado { get; set; }

    public bool? Lido { get; set; }

    public virtual CartaoControle? CartaoControle { get; set; }

    public virtual Email Email { get; set; } = null!;

    public virtual ICollection<NotificacoesDetalhe> NotificacoesDetalhes { get; } = new List<NotificacoesDetalhe>();

    public virtual Paciente? Paciente { get; set; }

    public virtual Usuario? Usuario { get; set; }
}
