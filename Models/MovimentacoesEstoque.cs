using System;
using System.Collections.Generic;

namespace authentication_jwt.Models;

public partial class MovimentacoesEstoque
{
    public long Id { get; set; }

    public DateTime DataMovimentacao { get; set; }

    public long MedicamentoId { get; set; }

    public long PacienteId { get; set; }

    public string TipoMovimentacao { get; set; } = null!;

    public decimal Quantidade { get; set; }

    public string? Observacao { get; set; }

    public long UsuarioId { get; set; }

    public virtual Medicamento Medicamento { get; set; } = null!;

    public virtual Paciente Paciente { get; set; } = null!;

    public virtual Usuario Usuario { get; set; } = null!;
}
