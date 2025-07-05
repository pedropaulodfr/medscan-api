using System;
using System.Collections.Generic;

namespace authentication_jwt.Models;

public partial class Tratamento
{
    public long Id { get; set; }

    public DateTime DataHoraCadastro { get; set; }

    public long UsuarioCadastroId { get; set; }

    public long PacienteId { get; set; }

    public string Identificacao { get; set; } = null!;

    public string? Descricao { get; set; }

    public string? Observacao { get; set; }

    public string? Patologia { get; set; }

    public string? Cid { get; set; }

    public string? ProfissionalResponsavel { get; set; }

    public DateTime? DataInicio { get; set; }

    public DateTime? DataFim { get; set; }

    public string Status { get; set; } = null!;

    public virtual Paciente Paciente { get; set; } = null!;

    public virtual ICollection<TratamentoReceituario> TratamentoReceituarios { get; } = new List<TratamentoReceituario>();

    public virtual Usuario UsuarioCadastro { get; set; } = null!;
}
