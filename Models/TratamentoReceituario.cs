using System;
using System.Collections.Generic;

namespace authentication_jwt.Models;

public partial class TratamentoReceituario
{
    public long Id { get; set; }

    public long TratamentoId { get; set; }

    public long ReceituarioId { get; set; }

    public virtual Receituario Receituario { get; set; } = null!;

    public virtual Tratamento Tratamento { get; set; } = null!;
}
