using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace authentication_jwt.DTO
{
    public class SolicitacoesDTO
    {
        public long? Id { get; set; }
        public DateTime? DataHoraSolicitacao { get; set; }
        public long? UsuarioSolicitacaoId { get; set; }
        public long? PacienteId { get; set; }
        public string? Paciente { get; set; }
        public string Identificacao { get; set; }
        public string? Descricao { get; set; }
        public long TipoMedicamentoId { get; set; }
        public string? TipoMedicamento { get; set; }
        public string Concentracao { get; set; }
        public long UnidadeId { get; set; }
        public string? Unidade { get; set; }
        public bool? Associacao { get; set; }
        public DateTime? DataHoraAnalise { get; set; }
        public long? UsuarioAnaliseId { get; set; }
        public string? Status { get; set; }
        public bool? Aprovado { get; set; }
        public bool? Deletado { get; set; }
    }
}