using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using authentication_jwt.DTO;
using authentication_jwt.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace authentication_jwt.Services
{
    public class ProcessamentoNotificacoesService : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;

        public ProcessamentoNotificacoesService(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }

        public async Task CriarNotificacoes()
        {
            try
            {
                using (var scope = _scopeFactory.CreateScope())
                {
                    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                    await dbContext.Database.ExecuteSqlRawAsync("EXEC CriaNotificacao");
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao criar notificações: " + ex.Message);
            }
        }

        public async Task<List<UsuarioNotificacaoDTO>> GetUsuariosNotificacoes()
        {
            try
            {
                using (var scope = _scopeFactory.CreateScope())
                {
                    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                    var usuarios = await dbContext.Set<UsuarioNotificacao>()
                        .FromSqlRaw("SELECT * FROM fn_GetUsuariosNotificacao()")
                        .Select(u => new UsuarioNotificacaoDTO
                        {
                            Notificacao_Id = u.Notificacao_Id,
                            DataRetorno = u.DataRetorno,
                            Medicamento = u.Medicamento,
                            Nome = u.Nome,
                            Email = u.Email,
                            Email2 = u.Email2
                        })
                        .ToListAsync();

                    return usuarios;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao criar notificações: " + ex.Message);
            }
        }

        public async Task ProcessarNotificacoes()
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                var emailService = scope.ServiceProvider.GetRequiredService<EmailService>();

                await CriarNotificacoes();
                var usuarios = await GetUsuariosNotificacoes();

                var emailTemplate = await dbContext.Emails.Where(x => x.Identificacao == "NotificacaoRetorno" && x.Ativo == true).FirstOrDefaultAsync();

                string titulo = string.Empty;
                string corpo = string.Empty;

                if (emailTemplate != null)
                {
                    titulo = emailTemplate.Titulo;
                    corpo = emailTemplate.Corpo;

                    foreach (var usuario in usuarios)
                    {
                        string nome = usuario.Nome;
                        string email = usuario.Email;
                        string medicamento = usuario.Medicamento;
                        string dataRetorno = usuario.DataRetorno.ToString("dd/MM/yyyy");

                        string body = corpo
                            .Replace("{NOME}", nome)
                            .Replace("{MEDICAMENTO}", medicamento)
                            .Replace("{DATARETORNO}", dataRetorno)
                            .Replace("{ICONEMEDICAMENTO}", "💊")
                            .Replace("{ICONECALENDARIO}", "📆");

                        await emailService.SendEmail(email, titulo, body);
                        if (!string.IsNullOrEmpty(usuario.Email2))
                            await emailService.SendEmail(usuario.Email2, titulo, body);
                            
                        var usuarioNotificacao = await dbContext.Notificacoes.Where(x => x.Id == usuario.Notificacao_Id).FirstOrDefaultAsync();
                        if (usuarioNotificacao != null)
                        {
                            usuarioNotificacao.Enviado = true;
                            await dbContext.SaveChangesAsync();
                        }
                    }
                }
                else
                {
                    throw new Exception("ERRO! Os E-mails não foram enviados, pois esse tipo de Notificação não possui um template ativo!");
                }
            }
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {

            Console.WriteLine($"Iniciando o serviço de notificações em {DateTimeOffset.Now.ToOffset(TimeSpan.FromHours(-3)).ToString("dd/MM/yyyy")} às {DateTimeOffset.Now.ToOffset(TimeSpan.FromHours(-3)).ToString("HH:mm:ss")}.");
            try
            {
                while (!stoppingToken.IsCancellationRequested)
                {
                    await ProcessarNotificacoes();
                    await Task.Delay(TimeSpan.FromHours(2), stoppingToken); // Adicionar delay de 2 horas
                }
            }
            catch (Exception ex)
            {
                throw new ArgumentException(ex.Message ?? ex.InnerException.ToString());
            }
        }
    }
}