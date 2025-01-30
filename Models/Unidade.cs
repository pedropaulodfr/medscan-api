using System;
using System.Collections.Generic;

namespace authentication_jwt.Models;

public partial class Unidade
{
    public long Id { get; set; }

    public string? Identificacao { get; set; }

    public string? Descricao { get; set; }

    public virtual ICollection<Medicamento> Medicamentos { get; } = new List<Medicamento>();
}
