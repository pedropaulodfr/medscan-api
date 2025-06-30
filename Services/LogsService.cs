using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using authentication_jwt.Models;

namespace authentication_jwt.Services
{
    public class LogsService
    {
        private readonly AppDbContext _dbContext;
        private readonly AcessoService _acesso;
        public LogsService(AppDbContext dbContext, AcessoService acesso)
        {
            _dbContext = dbContext;
            _acesso = acesso;
        }

        public async Task GravaLog(Log log)
        {
            try
            {
                var novoLog = new Log
                {
                    DataHora = DateTime.Now,
                    Acao = log.Acao,
                    JsonAntigo = log.JsonAntigo,
                    JsonNovo = log.JsonNovo,
                    UsuarioId = _acesso.UsuarioId
                };

                await _dbContext.AddAsync(novoLog);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new ArgumentException(ex.Message ?? ex.InnerException.ToString());
            }
        }
    }
}