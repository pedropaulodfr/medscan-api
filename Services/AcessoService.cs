using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using authentication_jwt.Models;

namespace authentication_jwt.Services
{
    public class AcessoService
    {
        private readonly AppDbContext _dbContext;
        private readonly IHttpContextAccessor _acesso;
        public long UsuarioId { get; }
        public string Email { get; }
        public string Perfil { get; }
        public long? PacienteId { get; }

        public AcessoService(AppDbContext dbContext, IHttpContextAccessor acesso)
        {
            _dbContext = dbContext;
            _acesso = acesso;

            if(!string.IsNullOrEmpty(_acesso.HttpContext?.User.FindFirst("Perfil")?.Value))
                Perfil = _acesso.HttpContext?.User.FindFirst("Perfil")?.Value;
            if(!string.IsNullOrEmpty(_acesso.HttpContext?.User.FindFirst("Email")?.Value))
                Email = _acesso.HttpContext?.User.FindFirst("Email")?.Value;
            if(!string.IsNullOrEmpty(_acesso.HttpContext?.User.FindFirst("PacienteId")?.Value))
                PacienteId = long.Parse(_acesso.HttpContext?.User.FindFirst("PacienteId")?.Value);
            if(!string.IsNullOrEmpty(_acesso.HttpContext?.User.FindFirst("UsuarioId")?.Value))
                UsuarioId = long.Parse(_acesso.HttpContext?.User.FindFirst("UsuarioId")?.Value);
        }
    }
}