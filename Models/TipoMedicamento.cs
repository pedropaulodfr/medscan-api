using System;
using System.Collections.Generic;

namespace authentication_jwt.Models;

public partial class TipoMedicamento
{
    public long Id { get; set; }

    public string Identificacao { get; set; } = null!;

    public string? Descricao { get; set; }

    public virtual ICollection<Medicamento> Medicamentos { get; } = new List<Medicamento>();

    public virtual ICollection<Receituario> Receituarios { get; } = new List<Receituario>();
}
