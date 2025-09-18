using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace authentication_jwt.DTO.Dashboard
{
    public class EstoqueMedicamentosDashboardDTO
    {
        public string? Medicamento { get; set; }
        public decimal? Quantidade { get; set; }
    }
}