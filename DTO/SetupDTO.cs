using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace authentication_jwt.DTO
{
    public class SetupDTO
    {
        public string? Urlapi { get; set; }

        public string? CaminhoArquivos { get; set; }

        public bool? UsarCodigoCadastro { get; set; }
    }
}