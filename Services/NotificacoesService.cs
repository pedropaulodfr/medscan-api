using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using authentication_jwt.DTO;
using authentication_jwt.Models;
using Microsoft.EntityFrameworkCore;

namespace authentication_jwt.Services
{
    public class NotificacoesService
    {
        private readonly AppDbContext _dbContext;
        private readonly AcessoService _acesso;

        // Construtor para injetar o AppDbContext
        public NotificacoesService(AppDbContext dbContext, AcessoService acesso)
        {
            _dbContext = dbContext;
            _acesso = acesso;
        }

        public async Task<List<UsuarioNotificacaoDTO>> GetAllNotificacoes()
        {
            try
            {
                var notificacoes = await _dbContext.Notificacoes
                                                    .Include(x => x.CartaoControle)
                                                        .ThenInclude(y => y.Medicamento)
                                                        .ThenInclude(z => z.Unidade)
                                                    .Include(x => x.Paciente)
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
                                                        Titulo = _dbContext.Emails.Where(e => e.Identificacao == n.Tipo && e.Ativo == true).Select(e => e.Titulo).FirstOrDefault()
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
    }
}