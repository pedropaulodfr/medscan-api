using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using authentication_jwt.DTO;
using authentication_jwt.Models;
using authentication_jwt.Utils;
using Microsoft.EntityFrameworkCore;

namespace authentication_jwt.Services
{
    public class NotificacoesService
    {
        private readonly AppDbContext _dbContext;
        private readonly AcessoService _acesso;
        private readonly Funcoes _funcoes;

        // Construtor para injetar o AppDbContext
        public NotificacoesService(AppDbContext dbContext, AcessoService acesso, Funcoes funcoes)
        {
            _dbContext = dbContext;
            _acesso = acesso;
            _funcoes = funcoes;
        }

        public async Task<List<UsuarioNotificacaoDTO>> GetAllNotificacoes()
        {
            try
            {
                var setup = await _dbContext.Setups.FirstOrDefaultAsync();

                var notificacoes = await _dbContext.Notificacoes
                                                    .Include(x => x.CartaoControle)
                                                        .ThenInclude(y => y.Medicamento)
                                                        .ThenInclude(z => z.Unidade)
                                                    .Include(x => x.Paciente)
                                                    .Include(x => x.Email)
                                                    .Include(x => x.NotificacoesDetalhes)
                                                    .Where(x => x.PacienteId == _acesso.PacienteId)
                                                    .OrderByDescending(x => x.Data)
                                                    .Select(n => new UsuarioNotificacaoDTO
                                                    {
                                                        Notificacao_Id = n.Id,
                                                        Data = (DateTime)n.Data,
                                                        Medicamento = $"{n.CartaoControle.Medicamento.Identificacao} {n.CartaoControle.Medicamento.Concentracao} {n.CartaoControle.Medicamento.Unidade.Identificacao}",
                                                        Nome = n.Paciente.Nome,
                                                        Email = n.Paciente.Email,
                                                        Tipo = n.Tipo,
                                                        Lido = n.Lido == null ? false : n.Lido,
                                                        Titulo = n.Email.Titulo,
                                                        NotificacaoDetalhes = n.NotificacoesDetalhes.Select(d => new NotificacoesDetalhesDTO
                                                        {
                                                            Id = d.Id,
                                                            NotificacoesId = d.NotificacoesId,
                                                            DataHoraEnvio = d.DataHoraEnvio,
                                                            TituloEnviado = d.TituloEnviado,
                                                            AssuntoEnviado = _funcoes.RemoverClassesHTML(d.AssuntoEnviado),
                                                            EnderecosEnviados = d.EnderecosEnviados,
                                                            EmailId = d.EmailId
                                                        }).FirstOrDefault()
                                                    }).ToListAsync();
                return notificacoes;
            }
            catch (Exception ex)
            {
                throw new ArgumentException(ex.Message ?? ex.InnerException.ToString());
            }
        }

        public async Task UpdateNotificacaoLida(long notificacaoId)
        {
            try
            {
                var notificacao = await _dbContext.Notificacoes.Where(x => x.Id == notificacaoId).FirstOrDefaultAsync();
                if (notificacao != null)
                {
                    notificacao.Lido = true;
                    await _dbContext.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                throw new ArgumentException(ex.Message ?? ex.InnerException.ToString());
            }
        }

        public async Task InsertDetalhesNotificacao(long notificacaoId, string titulo, string body, string email, long? emailId)
        {
            try
            {
                var detalhesNotificacao = new NotificacoesDetalhe
                {
                    DataHoraEnvio = DateTime.Now,
                    NotificacoesId = notificacaoId,
                    TituloEnviado = titulo,
                    AssuntoEnviado = body,
                    EnderecosEnviados = email,
                    EmailId = emailId
                };

                await _dbContext.AddAsync(detalhesNotificacao);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new ArgumentException(ex.Message ?? ex.InnerException.ToString());
            }
        }
    }
}