using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using authentication_jwt.DTO;
using authentication_jwt.DTO.Relatorios;
using authentication_jwt.Models;
using Microsoft.EntityFrameworkCore;

namespace authentication_jwt.Services
{
    public class RelatoriosService
    {
        private readonly AppDbContext _dbContext;
        public RelatoriosService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<RelatorioMedicamentosDTO>> RelatorioMedicamentos(RelatorioMedicamentosFiltros model)
        {
            bool inativo = !string.IsNullOrEmpty(model.Status) && model.Status == "Inativo";
            var medicamentos = await _dbContext.Medicamentos
                                    .Where(x => string.IsNullOrEmpty(model.Status) || x.Inativo == inativo)
                                    .AsNoTracking()
                                    .Select(m => new MedicamentoDTO
                {
                    Id = m.Id,
                    Identificacao = m.Identificacao,
                    Concentracao = m.Concentracao.ToString(),
                    Descricao = m.Descricao,
                    Unidade = (_dbContext.Unidades.Where(u => u.Id == m.UnidadeId).FirstOrDefault()).Identificacao,
                    UnidadeId = m.UnidadeId,
                    TipoMedicamento = (_dbContext.TipoMedicamentos.Where(tm => tm.Id == m.TipoMedicamentoId).FirstOrDefault()).Descricao,
                    TipoMedicamentoId = m.TipoMedicamentoId,
                    Associacao = m.Associacao,
                    Inativo = m.Inativo,
                    Status = m.Inativo == true ? "Inativo" : "Ativo"
                }).ToListAsync();

            var relatorioMedicamentos = medicamentos.Select(m => new RelatorioMedicamentosDTO
            {
                Medicamento = $"{m.Identificacao} {m.Concentracao} {m.Unidade}",
                Tipo = m.TipoMedicamento,
                Associacao = m.Associacao == true ? "Sim" : "Não",
                Status = m.Status,
            }).ToList();

            return relatorioMedicamentos;
        }

        public async Task<List<RelatorioReceituarioDTO>> RelatorioReceituario(long pacienteId)
        {
            var receituario = await _dbContext.Receituarios
                                        .Include(x => x.TipoMedicamento)
                                            .ThenInclude(y => y.Medicamentos)
                                                .ThenInclude(z => z.Unidade)
                                        .Where(x => x.PacienteId == pacienteId)
                                        .AsNoTracking().Select(m => new ReceituarioDTO
                                        {
                                            Id = m.Id,
                                            Frequencia = m.Frequencia,
                                            Tempo = m.Tempo,
                                            Periodo = m.Periodo,
                                            Dose = m.Dose,
                                            Medicamento = new MedicamentoDTO
                                            {
                                                Id = m.MedicamentoId,
                                                Identificacao = m.TipoMedicamento.Medicamentos.Where(x => x.Id == m.MedicamentoId).FirstOrDefault().Identificacao ?? "Sem Identificação",
                                                Descricao = m.TipoMedicamento.Medicamentos.Where(x => x.Id == m.MedicamentoId).FirstOrDefault().Descricao ?? "",
                                                Concentracao = m.TipoMedicamento.Medicamentos.Where(x => x.Id == m.MedicamentoId).FirstOrDefault().Concentracao.ToString() ?? "Sem Concentração",
                                                Unidade = m.TipoMedicamento.Medicamentos.Where(x => x.Id == m.MedicamentoId).FirstOrDefault().Unidade.Identificacao ?? "Sem Unidade",
                                                TipoMedicamento = m.TipoMedicamento.Identificacao,
                                                TipoMedicamentoId = m.TipoMedicamentoId,
                                            }
                                        }).ToListAsync();

            var relatorioReceituario = receituario.Select(x => new RelatorioReceituarioDTO
            {
                Medicamento = $"{x.Medicamento.Identificacao} {x.Medicamento.Concentracao} {x.Medicamento.Unidade}",
                Dose = $"{x.Dose} {x.Medicamento.TipoMedicamento}",
                Frequencia = $"{x.Frequencia} {(x.Frequencia > 1 ? "vezes" : "vez")} por {x.Tempo.ToLower()} pela {x.Periodo.ToLower()}"
            }).ToList();

            return relatorioReceituario;
        }
    }
}