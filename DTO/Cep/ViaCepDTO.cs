using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace authentication_jwt.DTO.Cep
{
    public class ViaCepDTO
    {
        public string? CEP { get; set; }
        public string? Logradouro { get; set; }
        public string? Numero { get; set; }
        public string? Complemento { get; set; }
        public string? Unidade { get; set; }
        public string? Bairro { get; set; }
        public string? Localidade { get; set; }
        public string? UF { get; set; }
        public string? Estado { get; set; }
        public string? Regiao { get; set; }
        public string? IBGE { get; set; }
        public string? GIA { get; set; }
        public string? DDD { get; set; }
        public string? siafi { get; set; }
    }
}