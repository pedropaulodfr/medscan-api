using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace authentication_jwt.DTO
{
    public class UploadArquivosS3DTO
    {
        public string? FileName { get; set; }
        public string? Base64 { get; set; }
        public string? Tipo { get; set; }
    }
}