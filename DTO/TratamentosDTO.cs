using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using authentication_jwt.Models;

namespace authentication_jwt.DTO
{
    public class TratamentosDTO
    {
        public long Id { get; set; }
        public DateTime? DataHoraCadastro { get; set; }
        public long? UsuarioCadastroId { get; set; }
        public long? PacienteId { get; set; }
        public string? Paciente { get; set; }
        public string Identificacao { get; set; }
        public string? Descricao { get; set; }
        public string? Observacao { get; set; }
        public string? Patologia { get; set; }
        public string? Cid { get; set; }
        public string? ProfissionalResponsavel { get; set; }
        public DateTime? DataInicio { get; set; }
        public DateTime? DataFim { get; set; }
        public string Status { get; set; }
        public List<ReceituarioDTO>? receituarios { get; set; }
    }
}