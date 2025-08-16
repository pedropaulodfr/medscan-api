using System;
using System.Collections.Generic;

namespace authentication_jwt.Models;

public partial class Setup
{
    public long Id { get; set; }

    public string? Urlapi { get; set; }

    public string? Urlweb { get; set; }

    public string? SmtpHost { get; set; }

    public string? SmtpPort { get; set; }

    public string? SmtpUser { get; set; }

    public string? SmtpPassword { get; set; }

    public string? CaminhoArquivos { get; set; }

    public bool? UsarCodigoCadastro { get; set; }

    public int? DiasNotificacaoRetorno { get; set; }

    public bool? AnaliseAutomatica { get; set; }

    public bool? PacienteAutocadastro { get; set; }

    public bool? PacienteCadastraReceituario { get; set; }

    public bool? PacienteCadastraCartaoControle { get; set; }

    public bool? PacienteCadastraTratamento { get; set; }
}
