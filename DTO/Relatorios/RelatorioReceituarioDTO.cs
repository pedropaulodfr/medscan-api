using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace authentication_jwt.DTO.Relatorios
{
    public class RelatorioReceituarioDTO
    {
        public string Medicamento { get; set; }
        public string Dose { get; set; }
        public string Frequencia { get; set; }
    }
}