using System;
using System.Collections.Generic;

namespace authentication_jwt.Models;

public partial class Setup
{
    public string? Urlapi { get; set; }

    public string? CaminhoArquivos { get; set; }

    public bool? UsarCodigoCadastro { get; set; }

    public string? SmtpHost { get; set; }

    public string? SmtpPort { get; set; }

    public string? SmtpUser { get; set; }

    public string? SmtpPassword { get; set; }
}
