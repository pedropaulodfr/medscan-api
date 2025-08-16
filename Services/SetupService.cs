using System.Collections.Generic;
using System.Text.Json;
using System.Web.Helpers;
using authentication_jwt.DTO;
using authentication_jwt.Models;
using Microsoft.EntityFrameworkCore;

namespace authentication_jwt.Services
{
    public class SetupService
    {
        private readonly AppDbContext _dbContext;
        private readonly LogsService _log;

        public SetupService(AppDbContext dbContext, LogsService log)
        {
            _dbContext = dbContext;
            _log = log;
        }

        public async Task<SetupDTO> Get()
        {
            return await _dbContext.Setups.Select(x => new SetupDTO
            {
                Urlapi = x.Urlapi,
                Urlweb = x.Urlweb,
                CaminhoArquivos = x.CaminhoArquivos,
                UsarCodigoCadastro = x.UsarCodigoCadastro,
                AnaliseAutomatica = x.AnaliseAutomatica,
                PacienteAutocadastro = x.PacienteAutocadastro,
                PacienteCadastraReceituario = x.PacienteCadastraReceituario,
                PacienteCadastraCartaoControle = x.PacienteCadastraCartaoControle,
                PacienteCadastraTratamento = x.PacienteCadastraTratamento,
                DiasNotificacaoRetorno = x.DiasNotificacaoRetorno,
                SmtpHost = x.SmtpHost,
                SmtpPort = x.SmtpPort,
                SmtpUser = x.SmtpUser,
                SmtpPassword = x.SmtpPassword
            }).FirstOrDefaultAsync();
        }

        public async Task<SetupDTO> UpdateSMTP(SetupDTO model)
        {
            try
            {
                var setup = await _dbContext.Setups.FirstOrDefaultAsync();
                if (setup == null)
                    throw new ArgumentException("Setup não encontrado!");

                var jsonAntigo = JsonSerializer.Serialize(new
                {
                    setup.SmtpHost,
                    setup.SmtpPort,
                    setup.SmtpUser,
                    setup.SmtpPassword,
                });

                setup.SmtpHost = model.SmtpHost;
                setup.SmtpPort = model.SmtpPort;
                setup.SmtpUser = model.SmtpUser;
                setup.SmtpPassword = model.SmtpPassword;

                await _dbContext.SaveChangesAsync();

                await _log.GravaLog(new Log {Acao = "Alteração SMTP", JsonAntigo = jsonAntigo, JsonNovo = JsonSerializer.Serialize(model)});

                return model;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message ?? ex.InnerException.ToString());
            }
        }
        public async Task<SetupDTO> Update(SetupDTO model)
        {
            try
            {
                var setup = await _dbContext.Setups.FirstOrDefaultAsync();
                if (setup == null)
                    throw new ArgumentException("Setup não encontrado!");

                var jsonAntigo = JsonSerializer.Serialize(new
                {
                    setup.Urlweb,
                    setup.Urlapi,
                    setup.CaminhoArquivos,
                    setup.DiasNotificacaoRetorno,
                    setup.UsarCodigoCadastro,
                    setup.AnaliseAutomatica,
                    setup.PacienteAutocadastro,
                    setup.PacienteCadastraReceituario,
                    setup.PacienteCadastraCartaoControle,
                    setup.PacienteCadastraTratamento
                });
                
                setup.Urlweb = model.Urlweb;
                setup.Urlapi = model.Urlapi;
                setup.CaminhoArquivos = model.CaminhoArquivos;
                setup.DiasNotificacaoRetorno = model.DiasNotificacaoRetorno;
                setup.UsarCodigoCadastro = model.UsarCodigoCadastro;
                setup.AnaliseAutomatica = model.AnaliseAutomatica;
                setup.PacienteAutocadastro = model.PacienteAutocadastro;
                setup.PacienteCadastraReceituario = model.PacienteCadastraReceituario;
                setup.PacienteCadastraCartaoControle = model.PacienteCadastraCartaoControle;
                setup.PacienteCadastraTratamento = model.PacienteCadastraTratamento;

                await _dbContext.SaveChangesAsync();

                await _log.GravaLog(new Log {Acao = "Alteração Setup", JsonAntigo = jsonAntigo, JsonNovo = JsonSerializer.Serialize(model)});
                
                return model;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message ?? ex.InnerException.ToString());
            }
        }
    }
}
