using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace authentication_jwt.DTO
{
    public class NotificacoesDetalhesDTO
    {
        public long Id { get; set; }
        public long NotificacoesId { get; set; }
        public DateTime DataHoraEnvio { get; set; }
        public string? TituloEnviado { get; set; }
        public string? AssuntoEnviado { get; set; }
        public string? EnderecosEnviados { get; set; }
        public long? EmailId { get; set; }
    }
}