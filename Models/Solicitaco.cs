using System;
using System.Collections.Generic;

namespace authentication_jwt.Models;

public partial class Solicitaco
{
    public long Id { get; set; }

    public DateTime DataHoraSolicitacao { get; set; }

    public long UsuarioSolicitacaoId { get; set; }

    public long PacienteId { get; set; }

    public string Identificacao { get; set; } = null!;

    public string? Descricao { get; set; }

    public long TipoMedicamentoId { get; set; }

    public string Concentracao { get; set; } = null!;

    public long UnidadeId { get; set; }

    public bool? Associacao { get; set; }

    public DateTime? DataHoraAnalise { get; set; }

    public long? UsuarioAnaliseId { get; set; }

    public bool? Aprovado { get; set; }

    public bool? Deletado { get; set; }

    public virtual Paciente Paciente { get; set; } = null!;

    public virtual TipoMedicamento TipoMedicamento { get; set; } = null!;

    public virtual Unidade Unidade { get; set; } = null!;

    public virtual Usuario? UsuarioAnalise { get; set; }

    public virtual Usuario UsuarioSolicitacao { get; set; } = null!;
}
