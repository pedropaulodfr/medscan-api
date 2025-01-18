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
        
        public string? SmtpHost { get; set; }

        public string? SmtpPort { get; set; }

        public string? SmtpUser { get; set; }

        public string? SmtpPassword { get; set; }
    }
}