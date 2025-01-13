using System;
using System.Collections.Generic;

namespace authentication_jwt.Models;

public partial class CartaoControle
{
    public long Id { get; set; }

    public long MedicamentoId { get; set; }

    public long? Quantidade { get; set; }

    public DateTime? Data { get; set; }

    public DateTime? DataRetorno { get; set; }

    public string? Profissional { get; set; }

    public long PacienteId { get; set; }

    public virtual Medicamento Medicamento { get; set; } = null!;

    public virtual Paciente Paciente { get; set; } = null!;
}
