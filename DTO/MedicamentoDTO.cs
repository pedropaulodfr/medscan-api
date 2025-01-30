using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace authentication_jwt.DTO
{
    public class MedicamentoDTO
    {
        public long Id { get; set; }
        public string? Identificacao { get; set; }
        public string? Descricao { get; set; }
        public string? Concentracao { get; set; }
        public long? TipoMedicamentoId { get; set; }
        public string? TipoMedicamento { get; set; }
        public long? UnidadeId { get; set; }
        public string? Unidade { get; set; }
        public bool? Associacao { get; set; }
        public bool? Inativo { get; set; }
        public string? Status { get; set; }
    }
}