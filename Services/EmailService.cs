using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Mail;
using System.Net;
using authentication_jwt.Models;
using Microsoft.EntityFrameworkCore;
using authentication_jwt.DTO;


namespace authentication_jwt.Services
{
    public class EmailService
    {
        private readonly AppDbContext _dbContext;

        public EmailService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<EmailDTO>> GetAll()
        {
            var emails = await _dbContext.Emails.ToListAsync();
            var retorno = emails.Select(e => new EmailDTO
            {
                Id = e.Id,
                Identificacao = e.Identificacao,
                Perfil = e.Perfil,
                Descricao = e.Descricao,
                Titulo = e.Titulo,
                Corpo = e.Corpo,
                Ativo = e.Ativo,
                Status = e.Ativo == true ? "Ativo" : "Inativo"
            }).ToList();

            return retorno;
        }

        public async Task<EmailDTO> Update(EmailDTO model)
        {
            try
            {
                var email = await _dbContext.Emails.Where(x => x.Id == model.Id).FirstOrDefaultAsync();
                if (email == null)
                    throw new ArgumentException("Email não encontrado!");

                email.Identificacao = model.Identificacao;
                email.Perfil = model.Perfil;
                email.Descricao = model.Descricao;
                email.Titulo = model.Titulo;
                email.Corpo = model.Corpo;
                email.Ativo = model.Status == "Ativo" ? true : false;

                await _dbContext.SaveChangesAsync();

                return model;
            }
            catch (Exception ex)
            {
                throw new ArgumentException(ex.Message ?? ex.InnerException.ToString());
            }
        }

        public async Task EnviarEmailNovoCadastro(string email, string nome, string senha)
        {
            try
            {
                var setup = await _dbContext.Setups.FirstOrDefaultAsync();
                var setupEmail = await _dbContext.Emails.Where(x => x.Identificacao == "NovoCadastro" && x.Ativo == true).FirstOrDefaultAsync();
                if (setupEmail == null)
                    throw new ArgumentException("Configuração de email não encontrada");

                var body = setupEmail?.Corpo.Replace("{NOME}", nome).Replace("{SENHA}", senha).Replace("{USUARIO}", email) ?? "";

                await SendEmail(email, setupEmail.Titulo, body);
            }
            catch (Exception ex)
            {
                throw new ArgumentException(ex.Message ?? ex.InnerException.ToString());
            }
        }
        public async Task EnviarEmailEsqueceuSenha(string email, string nome, string senha)
        {
            try
            {
                var setup = await _dbContext.Setups.FirstOrDefaultAsync();
                var setupEmail = await _dbContext.Emails.Where(x => x.Identificacao == "RecuperarSenha" && x.Ativo == true).FirstOrDefaultAsync();
                if (setupEmail == null)
                    throw new ArgumentException("Configuração de email não encontrada");

                var body = setupEmail?.Corpo.Replace("{NOME}", nome).Replace("{SENHA}", senha) ?? "";

                await SendEmail(email, setupEmail.Titulo, body);
            }
            catch (Exception ex)
            {
                throw new ArgumentException(ex.Message ?? ex.InnerException.ToString());
            }
        }

        public async Task<MailMessage> SendEmail(string toEmail, string titulo, string body)
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
                    body = body
                        .Replace("{URLWeb}", setup.Urlweb)
                        .Replace("{URLApi}", setup.Urlapi)
                        .Replace("{ICONEMEDICAMENTO}", "💊")
                        .Replace("{ICONECALENDARIO}", "📆");

                    var mailMessage = new MailMessage
                    {
                        From = new MailAddress(setup.SmtpUser),
                        Subject = titulo,
                        Body = body,
                        IsBodyHtml = true
                    };

                    foreach (var email in toEmail.Split(',', StringSplitOptions.RemoveEmptyEntries))
                    {
                        mailMessage.To.Add(email.Trim());
                    }

                    await smtpClient.SendMailAsync(mailMessage);

                    return mailMessage;
                }
            }
            catch (Exception ex)
            {
                throw new ArgumentException(ex.Message ?? ex.InnerException.ToString());
            }
        }
    }
}