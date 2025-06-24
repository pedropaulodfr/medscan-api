using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace authentication_jwt.Models
{
    public class UsuarioNotificacao
    {
        public long Notificacao_Id  { get; set; }
        public DateTime DataRetorno { get; set; }
        public string Medicamento { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }
        public string Email2 { get; set; }
    }
}