using System;
using System.Collections.Generic;

namespace authentication_jwt.Models;

public partial class Receituario
{
    public long Id { get; set; }

    public long MedicamentoId { get; set; }

    public int? Frequencia { get; set; }

    public string? Tempo { get; set; }

    /// <summary>
    /// 1 - Manhã; 2 - Tarde; 3 - Noite
    /// </summary>
    public string? Periodo { get; set; }

    public int? Dose { get; set; }

    public long TipoMedicamentoId { get; set; }

    public long PacienteId { get; set; }

    public virtual Medicamento Medicamento { get; set; } = null!;

    public virtual TipoMedicamento TipoMedicamento { get; set; } = null!;
}
