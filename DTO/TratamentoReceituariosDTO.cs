using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace authentication_jwt.DTO
{
    public class TratamentoReceituariosDTO
    {
        public long? Id { get; set; }
        public long? TratamentoId { get; set; }
        public long? ReceituarioId { get; set; }
    }
}