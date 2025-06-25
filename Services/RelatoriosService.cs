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
            var medicamentos = _dbContext.Medicamentos.AsNoTracking().Where(x => x.Inativo == inativo).Select(m => new MedicamentoDTO
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
                }).ToList();

            var relatorioMedicamentos = medicamentos.Select(m => new RelatorioMedicamentosDTO
            {
                Medicamento = $"{m.Identificacao} {m.Concentracao} {m.Unidade}",
                Tipo = m.TipoMedicamento,
                Associacao = m.Associacao == true ? "Sim" : "Não",
                Status = m.Status,
            }).ToList();

            return relatorioMedicamentos;
        }
    }
}