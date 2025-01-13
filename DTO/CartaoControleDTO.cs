using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace authentication_jwt.DTO
{
    public class CartaoControleDTO
    {
        public long Id { get; set; }

        public string Medicamento { get; set; }
        public long MedicamentoId { get; set; }

        public string? Concentracao { get; set; }
        public string? Unidade { get; set; }
        public long? Quantidade { get; set; }
        public string? Tipo { get; set; }

        public DateTime? Data { get; set; }

        public DateTime? DataRetorno { get; set; }

        public string? Profissional { get; set; }
        public long? PacienteId { get; set; }
        public long? UsuarioId { get; set; }
    }
}