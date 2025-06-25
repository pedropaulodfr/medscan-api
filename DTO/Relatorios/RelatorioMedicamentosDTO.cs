using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace authentication_jwt.DTO.Relatorios
{
    public class RelatorioMedicamentosDTO
    {
        public string Medicamento { get; set; }
        public string Tipo { get; set; }
        public string Status { get; set; }
        public string Associacao { get; set; }
    }
    public class RelatorioMedicamentosFiltros
    {
        public string? Status { get; set; }
    }
}