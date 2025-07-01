using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using authentication_jwt.DTO;
using authentication_jwt.Models;
using Microsoft.EntityFrameworkCore;

namespace authentication_jwt.Services
{
    public class SolicitacoesService
    {
        private readonly AppDbContext _dbContext;
        private readonly AcessoService _acesso;
        private readonly MedicamentosService _medicamentos;
        private readonly LogsService _log;

        public SolicitacoesService(AppDbContext dbContext, AcessoService acesso, MedicamentosService medicamentos, LogsService log)
        {
            _dbContext = dbContext;
            _acesso = acesso;
            _medicamentos = medicamentos;
            _log = log;
        }

        public async Task<List<SolicitacoesDTO>> GetAll(string Status, long? PacienteId)
        {
            var solicitacoes = await _dbContext.Solicitacoes
                                        .Include(x => x.Unidade)
                                        .Include(x => x.TipoMedicamento)
                                        .Include(x => x.Paciente)
                                        .Where(x =>
                                                x.Deletado != true &&
                                                (Status == "Todos" ||
                                                (Status == "Aprovado" && x.Aprovado == true) ||
                                                (Status == "Reprovado" && x.Aprovado == false) ||
                                                (Status == "Em Análise" && x.Aprovado == null)) &&
                                                ((PacienteId == null || PacienteId == 0) || x.PacienteId == PacienteId)
                                                )
                                        .Select(s => new SolicitacoesDTO
                                        {
                                            Id = s.Id,
                                            DataHoraSolicitacao = s.DataHoraSolicitacao,
                                            UsuarioSolicitacaoId = s.UsuarioSolicitacaoId,
                                            PacienteId = s.PacienteId,
                                            Paciente = s.Paciente.NomeCompleto,
                                            Identificacao = s.Identificacao,
                                            Descricao = s.Descricao,
                                            TipoMedicamentoId = s.TipoMedicamentoId,
                                            TipoMedicamento = s.TipoMedicamento.Identificacao,
                                            Concentracao = s.Concentracao,
                                            UnidadeId = s.UnidadeId,
                                            Unidade = s.Unidade.Identificacao,
                                            Associacao = s.Associacao,
                                            DataHoraAnalise = s.DataHoraAnalise,
                                            UsuarioAnaliseId = s.UsuarioAnaliseId,
                                            Status = s.Aprovado == true ? "Aprovado" : s.Aprovado == false ? "Reprovado" : "Em Análise",
                                            Aprovado = s.Aprovado,
                                            Deletado = s.Deletado
                                        }).AsNoTracking().ToListAsync();

            return solicitacoes;
        }

        public async Task<SolicitacoesDTO> Insert(SolicitacoesDTO model)
        {
            try
            {
                Solicitaco solicitacao = new Solicitaco
                {
                    DataHoraSolicitacao = DateTime.Now,
                    UsuarioSolicitacaoId = _acesso.UsuarioId,
                    PacienteId = model.PacienteId.Value,
                    Identificacao = model.Identificacao,
                    Descricao = model.Descricao,
                    TipoMedicamentoId = model.TipoMedicamentoId,
                    Concentracao = model.Concentracao,
                    UnidadeId = model.UnidadeId,
                    Associacao = model.Associacao
                };

                await _dbContext.AddAsync(solicitacao);
                await _dbContext.SaveChangesAsync();

                return model;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message ?? ex.InnerException.ToString());
            }
        }

        public async Task AnaliseSolicitacao(long SolicitacaoId, bool Aprovado)
        {
            try
            {
                var solicitacao = await _dbContext.Solicitacoes.Where(x => x.Id == SolicitacaoId).FirstOrDefaultAsync();
                if (solicitacao == null)
                    throw new ArgumentException("Solicitação inexistente!");

                solicitacao.DataHoraAnalise = DateTime.Now;
                solicitacao.UsuarioAnaliseId = _acesso.UsuarioId;

                if (Aprovado)
                {
                    solicitacao.Aprovado = true;

                    MedicamentoDTO medicamento = new MedicamentoDTO
                    {
                        Identificacao = solicitacao.Identificacao,
                        Descricao = solicitacao.Descricao,
                        Concentracao = solicitacao.Concentracao,
                        TipoMedicamentoId = solicitacao.TipoMedicamentoId,
                        UnidadeId = solicitacao.UnidadeId,
                        Associacao = solicitacao.Associacao,
                        Solicitado = true
                    };

                    await _medicamentos.Insert(medicamento);
                }
                else
                    solicitacao.Aprovado = false;

                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message ?? ex.InnerException.ToString());
            }
        }

        public async Task Delete(long SolicitacaoId)
        {
            try
            {
                var solicitacao = await _dbContext.Solicitacoes.Where(x => x.Id == SolicitacaoId && x.Aprovado != true).FirstOrDefaultAsync();
                if (solicitacao == null)
                    throw new Exception("Erro ao deletar, a solicitação não existe ou já foi aprovada!");

                solicitacao.Deletado = true;
                await _dbContext.SaveChangesAsync();

                await _log.GravaLog(new Log { Acao = "Deletar Solicitação", JsonAntigo = null, JsonNovo = null });
            }
            catch (Exception ex)
            {
                throw new ArgumentException(ex.Message ?? ex.InnerException.ToString());
            }
        }
    }
}