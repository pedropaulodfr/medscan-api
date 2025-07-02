using System;
using System.Collections.Generic;

namespace authentication_jwt.Models;

public partial class NotificacoesDetalhe
{
    public long Id { get; set; }

    public long NotificacoesId { get; set; }

    public DateTime DataHoraEnvio { get; set; }

    public string? TituloEnviado { get; set; }

    public string? AssuntoEnviado { get; set; }

    public string? EnderecosEnviados { get; set; }

    public long? EmailId { get; set; }

    public virtual Email? Email { get; set; }

    public virtual Notificaco Notificacoes { get; set; } = null!;
}
