using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace authentication_jwt.DTO
{
    public class NotificacaoDTO
    {
        public long Id { get; set; }
        public DateTime? Data { get; set; }
        public string? Tipo { get; set; }
        public long? CartaoControleId { get; set; }
        public long EmailId { get; set; }
        public long UsuarioId { get; set; }
        public long PacienteId { get; set; }
        public bool? Enviado { get; set; }
        public bool? Lido { get; set; }
    }
}