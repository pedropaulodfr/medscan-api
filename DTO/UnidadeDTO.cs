using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace authentication_jwt.DTO
{
    public class UnidadeDTO
    {
        public long Id { get; set; }
        public string Identificacao { get; set; }
        public string? Descricao { get; set; }
    }
}