using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Mail;
using System.Net;
using authentication_jwt.Models;
using Microsoft.EntityFrameworkCore;


namespace authentication_jwt.Services
{
    public class EmailService
    {
        private readonly AppDbContext _dbContext;

        public EmailService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task EnviarEmailNovoCadastro(string email, string nome, string senha)
        {
            var setup = await _dbContext.Setups.FirstOrDefaultAsync();
            var setupEmail = await _dbContext.Emails.Where(x => x.Identificacao == "NovoCadastro").FirstOrDefaultAsync();
            if (setupEmail == null)
                throw new ArgumentException("Configuração de email não encontrada");

            var body = setupEmail?.Corpo.Replace("{NOME}", nome).Replace("{SENHA}", senha).Replace("{USUARIO}", email)
                                        .Replace("{URLApi}", "https://medscan-web.fly.dev/") ?? "";

            try
            {
                await SendEmail(email, setupEmail.Titulo, body);
            }
            catch (Exception ex)
            {
                throw new ArgumentException(ex.Message ?? ex.InnerException.ToString());
            }
        }
        public async Task EnviarEmailEsqueceuSenha(string email, string nome, string senha)
        {
            var setup = await _dbContext.Setups.FirstOrDefaultAsync();
            var setupEmail = await _dbContext.Emails.Where(x => x.Identificacao == "RecuperarSenha").FirstOrDefaultAsync();
            if (setupEmail == null)
                throw new ArgumentException("Configuração de email não encontrada");

            var body = setupEmail?.Corpo.Replace("{NOME}", nome).Replace("{SENHA}", senha).Replace("{URLApi}", "https://medscan-web.fly.dev/") ?? "";

            try
            {
                await SendEmail(email, setupEmail.Titulo, body);
            }
            catch (Exception ex)
            {
                throw new ArgumentException(ex.Message ?? ex.InnerException.ToString());
            }
        }

        public async Task SendEmail(string toEmail, string titulo, string body)
        {
            try
            {
                var setup = await _dbContext.Setups.FirstOrDefaultAsync();
                if (setup == null)
                    throw new ArgumentException("Configuração de email não encontrada");

                using (var smtpClient = new SmtpClient(setup.SmtpHost, int.Parse(setup.SmtpPort)))
                {
                    smtpClient.Credentials = new NetworkCredential(setup.SmtpUser, setup.SmtpPassword);
                    smtpClient.EnableSsl = true;

                    var mailMessage = new MailMessage
                    {
                        From = new MailAddress(setup.SmtpUser),
                        Subject = titulo,
                        Body = body,
                        IsBodyHtml = true
                    };

                    mailMessage.To.Add(toEmail);

                    await smtpClient.SendMailAsync(mailMessage);
                }
            }
            catch (Exception ex)
            {
                throw new ArgumentException(ex.Message ?? ex.InnerException.ToString());
            }
        }
    }
}