using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace authentication_jwt.DTO
{
    public class EmailDTO
    {
        public long Id { get; set; }

        public string? Identificacao { get; set; }

        public string? Perfil { get; set; }

        public string? Descricao { get; set; }

        public string? Titulo { get; set; }

        public string? Corpo { get; set; }

        public bool? Ativo { get; set; }
        public string? Status { get; set; }
    }
}