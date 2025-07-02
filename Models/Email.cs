using System;
using System.Collections.Generic;

namespace authentication_jwt.Models;

public partial class Email
{
    public long Id { get; set; }

    public string? Identificacao { get; set; }

    public string? Perfil { get; set; }

    public string? Descricao { get; set; }

    public string? Titulo { get; set; }

    public string? Corpo { get; set; }

    public bool? Ativo { get; set; }

    public virtual ICollection<NotificacoesDetalhe> NotificacoesDetalhes { get; } = new List<NotificacoesDetalhe>();

    public virtual ICollection<Notificaco> Notificacos { get; } = new List<Notificaco>();
}
