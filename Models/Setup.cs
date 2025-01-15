using System;
using System.Collections.Generic;

namespace authentication_jwt.Models;

public partial class Setup
{
    public string? Urlapi { get; set; }

    public string? CaminhoArquivos { get; set; }

    public bool? UsarCodigoCadastro { get; set; }
}
