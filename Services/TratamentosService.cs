using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using authentication_jwt.DTO;
using authentication_jwt.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Update.Internal;

namespace authentication_jwt.Services
{
    public class TratamentosService
    {
        private readonly AppDbContext _dbContext;
        private readonly AcessoService _acesso;
        public TratamentosService(AppDbContext dbContext, AcessoService acesso)
        {
            _dbContext = dbContext;
            _acesso = acesso;
        }

        public async Task<List<TratamentosDTO>> Get(long? PacienteId)
        {
            var tratamentos = await _dbContext.Tratamentos
            .Include(x => x.Paciente)
            .Include(x => x.TratamentoReceituarios)
                .ThenInclude(y => y.Receituario)
                .ThenInclude(z => z.Medicamento)
                .ThenInclude(z => z.TipoMedicamento)
            .Where(x => x.Status != "Deletado" && ((PacienteId == null || PacienteId == 0) || x.PacienteId == PacienteId)
            ).AsNoTracking().Select(x => new TratamentosDTO
            {
                Id = x.Id,
                DataHoraCadastro = x.DataHoraCadastro,
                PacienteId = x.PacienteId,
                Paciente = x.Paciente.NomeCompleto,
                Identificacao = x.Identificacao,
                Descricao = x.Descricao,
                Observacao = x.Observacao,
                Patologia = x.Patologia,
                Cid = x.Cid,
                ProfissionalResponsavel = x.ProfissionalResponsavel,
                DataInicio = x.DataInicio,
                DataFim = x.DataFim,
                Status = x.Status,
                receituarios = x.TratamentoReceituarios.Select(r => new ReceituarioDTO
                {
                    Id = r.Receituario.Id,
                    Frequencia = r.Receituario.Frequencia,
                    Tempo = r.Receituario.Tempo,
                    Periodo = r.Receituario.Periodo,
                    Dose = r.Receituario.Dose,
                    Medicamento = new MedicamentoDTO
                    {
                        Id = r.Receituario.Medicamento.Id,
                        Identificacao = r.Receituario.Medicamento.Identificacao,
                        Descricao = r.Receituario.Medicamento.Descricao,
                        Concentracao = r.Receituario.Medicamento.Concentracao,
                        TipoMedicamentoId = r.Receituario.Medicamento.TipoMedicamentoId,
                        TipoMedicamento = r.Receituario.Medicamento.TipoMedicamento.Identificacao,
                        UnidadeId = r.Receituario.Medicamento.UnidadeId,
                        Unidade = r.Receituario.Medicamento.Unidade.Identificacao,
                        Associacao = r.Receituario.Medicamento.Associacao,
                        Inativo = r.Receituario.Medicamento.Inativo,
                        Solicitado = r.Receituario.Medicamento.Solicitado,
                        SolicitadoString = r.Receituario.Medicamento.Solicitado == true ? "Sim" : "Não"
                    }
                }).ToList()
            }).ToListAsync();

            return tratamentos;
        }

        public async Task<TratamentosDTO> Insert(TratamentosDTO model)
        {
            try
            {
                Tratamento existTratamento = await _dbContext.Tratamentos.Where(x => x.Identificacao == model.Identificacao && x.PacienteId == _acesso.PacienteId.GetValueOrDefault()).FirstOrDefaultAsync();
                if (existTratamento != null)
                    throw new ArgumentException("Este tratamento já foi cadastrado para o paciente");

                Tratamento tratamento = new Tratamento
                {
                    DataHoraCadastro = DateTime.Now,
                    UsuarioCadastroId = _acesso.UsuarioId,
                    PacienteId = _acesso.PacienteId.Value,
                    Identificacao = model.Identificacao,
                    Descricao = model.Descricao,
                    Observacao = model.Observacao,
                    Patologia = model.Patologia,
                    Cid = model.Cid,
                    ProfissionalResponsavel = model.ProfissionalResponsavel,
                    DataInicio = model.DataInicio,
                    DataFim = model.DataFim,
                    Status = model.Status
                };

                await _dbContext.AddAsync(tratamento);
                await _dbContext.SaveChangesAsync();

                foreach (var receituario in model.receituarios)
                {
                    TratamentoReceituario tratamentoReceituario = new TratamentoReceituario
                    {
                        ReceituarioId = receituario.Id,
                        TratamentoId = tratamento.Id
                    };
                    await _dbContext.AddAsync(tratamentoReceituario);
                }

                await _dbContext.SaveChangesAsync();

                return model;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message ?? ex.InnerException.ToString());
            }
        }

        public async Task<TratamentosDTO> Update(TratamentosDTO model)
        {
            try
            {
                var tratamentoExistente = await _dbContext.Tratamentos.Include(x => x.TratamentoReceituarios).Where(x => x.Id == model.Id).FirstOrDefaultAsync();
                if (tratamentoExistente == null)
                    throw new Exception("O tratamento não existe!");

                tratamentoExistente.Identificacao = model.Identificacao;
                tratamentoExistente.Descricao = model.Descricao;
                tratamentoExistente.Observacao = model.Observacao;
                tratamentoExistente.Patologia = model.Patologia;
                tratamentoExistente.Cid = model.Cid;
                tratamentoExistente.ProfissionalResponsavel = model.ProfissionalResponsavel;
                tratamentoExistente.DataInicio = model.DataInicio;
                tratamentoExistente.DataFim = model.DataFim;
                tratamentoExistente.Status = model.Status;

                // Lista de IDs de receituários enviados no model
                var idsReceituariosModel = model.receituarios.Select(r => r.Id).ToList();

                // Remove vínculos que não estão mais no model
                var paraRemover = tratamentoExistente.TratamentoReceituarios
                    .Where(tr => !idsReceituariosModel.Contains(tr.ReceituarioId))
                    .ToList();

                foreach (var tr in paraRemover)
                {
                    _dbContext.TratamentoReceituarios.Remove(tr);
                }

                // Adiciona novos vínculos que não existem ainda
                var idsExistentes = tratamentoExistente.TratamentoReceituarios.Select(tr => tr.ReceituarioId).ToList();
                foreach (var receituario in model.receituarios)
                {
                    if (!idsExistentes.Contains(receituario.Id))
                    {
                        TratamentoReceituario novo = new TratamentoReceituario
                        {
                            ReceituarioId = receituario.Id,
                            TratamentoId = tratamentoExistente.Id
                        };
                        await _dbContext.AddAsync(novo);
                    }
                }

                await _dbContext.SaveChangesAsync();

                return model;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message ?? ex.InnerException.ToString());
            }
        }
        
        public async Task Delete(long id)
        {
            try
            {
                var tratamento = await _dbContext.Tratamentos.Include(x => x.TratamentoReceituarios).Where(u => u.Id == id).FirstOrDefaultAsync();
                if (tratamento == null)
                    throw new Exception("O tratamento não existe!");

                _dbContext.RemoveRange(tratamento.TratamentoReceituarios);
                _dbContext.Remove(tratamento);
                
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message ?? ex.InnerException.ToString());
            }
        }
    }
}