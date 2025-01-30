using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace authentication_jwt.DTO.Dashboard
{
    public class CardsDashboardDTO
    {
        public DateTime? DataRetorno { get; set; }
        public int? Quantidade { get; set; }
        public long? CartaoControleId { get; set; }
        public string? Medicamento { get; set; }
    }
}