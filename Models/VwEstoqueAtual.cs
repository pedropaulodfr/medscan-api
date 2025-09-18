using System;
using System.Collections.Generic;

namespace authentication_jwt.Models;

public partial class VwEstoqueAtual
{
    public long MedicamentoId { get; set; }

    public string Identificacao { get; set; } = null!;

    public decimal? QuantidadeAtual { get; set; }
}
