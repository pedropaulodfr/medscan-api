using authentication_jwt.DTO;
using authentication_jwt.DTO.Dashboard;
using authentication_jwt.Models;
using Microsoft.EntityFrameworkCore;

namespace authentication_jwt.Services
{
    public class DashboardService
    {
        private readonly AppDbContext _dbContext;
        private readonly AcessoService _acesso;

        public DashboardService(AppDbContext dbContext, AcessoService acesso)
        {
            _dbContext = dbContext;
            _acesso = acesso;
        }

        public async Task<List<CartaoControleDashboardDTO>> CartaoControle(long? PacienteId)
        {
            if (PacienteId == null || PacienteId == 0)
                PacienteId = _acesso.PacienteId;

            var dados = await _dbContext.CartaoControles.Where(x => x.PacienteId == PacienteId).AsNoTracking().ToListAsync();

            var retorno = dados.GroupBy(x => x.DataRetorno ).Select(y => new CartaoControleDashboardDTO
            {
                DataRetorno = y.Key,
                Quantidade = y.Count()
            }).ToList();

            return retorno;
        }

        public async Task<List<CardsDashboardDTO>> CardProximoRetorno(long? PacienteId)
        {
            if (PacienteId == null || PacienteId == 0)
                PacienteId = _acesso.PacienteId;

            var dados = await _dbContext.CartaoControles
                                            .Include(x => x.Medicamento)
                                                .ThenInclude(y => y.Unidade)
                                            .AsNoTracking()
                                            .Where(x => x.DataRetorno >= DateTime.Now && x.DataRetorno <= DateTime.Now.AddDays(30)
                                                    && x.PacienteId == PacienteId
                                                    && x.Paciente.Deletado != true
                                                    && x.Medicamento.Inativo != true)
                                            .ToListAsync();

            // Agrupa os dados por DataRetorno
            var agrupadosPorDataRetorno = dados.GroupBy(x => x.DataRetorno)
                                            .Select(grupo => new
                                            {
                                                DataRetorno = grupo.Key,
                                                Medicamentos = grupo.ToList(),
                                                Quantidade = grupo.Count()
                                            })
                                            .ToList();

            var proximosAoRetorno = agrupadosPorDataRetorno.SelectMany(grupo => grupo.Medicamentos.Select(medicamento => new CardsDashboardDTO
            {
                CartaoControleId = medicamento.Id,
                DataRetorno = medicamento.DataRetorno,
                Medicamento = string.Format("{0} {1} {2}", 
                                            medicamento.Medicamento.Identificacao, 
                                            medicamento.Medicamento.Concentracao, 
                                            medicamento.Medicamento.Unidade.Identificacao),
                Quantidade = grupo.Quantidade 
            })).ToList();

            return proximosAoRetorno;
        }
        public async Task<List<MedicamentoDTO>> CardQntMedicamentosPaciente(long? PacienteId)
        {
            if (PacienteId == null || PacienteId == 0)
                PacienteId = _acesso.PacienteId;

            var receituario = await _dbContext.Receituarios.Include(x => x.Medicamento).Where(x => x.PacienteId == PacienteId && x.Medicamento.Inativo != true).Select(m => new MedicamentoDTO
            {
                Id = m.Id,
                Identificacao = m.Medicamento.Identificacao,
                Concentracao = m.Medicamento.Concentracao.ToString(),
                Descricao = m.Medicamento.Descricao,
                Unidade = (_dbContext.Unidades.Where(u => u.Id == m.Medicamento.UnidadeId).FirstOrDefault()).Identificacao ?? "",
                UnidadeId = m.Medicamento.UnidadeId,
                TipoMedicamento = (_dbContext.TipoMedicamentos.Where(tm => tm.Id == m.Medicamento.TipoMedicamentoId).FirstOrDefault()).Descricao ?? "",
                TipoMedicamentoId = m.TipoMedicamentoId,
                Associacao = m.Medicamento.Associacao,
                Inativo = m.Medicamento.Inativo,
                Status = m.Medicamento.Inativo == true ? "Inativo" : "Ativo"
            }).ToListAsync();

            return receituario;
        }

        public async Task<List<EstoqueMedicamentosDashboardDTO>> EstoqueMedicamentos(long? PacienteId)
        {
            if (PacienteId == null || PacienteId == 0)
                PacienteId = _acesso.PacienteId;
                
            var dados = await _dbContext.CartaoControles
                                        .Include(x => x.Medicamento)
                                            .ThenInclude(y => y.Unidade)
                                        .Where(x => x.PacienteId == PacienteId 
                                                && x.Paciente.Deletado != true 
                                                && x.Medicamento.Inativo != true
                                                && x.Data <= DateTime.Now )
                                        .AsNoTracking().ToListAsync();

            /* CC = Cartão de Controle | R = Receituário
            Fórmula para calcular a quantidade:
            Quantidade = CC.quantidade - ((dataHoje - CC.data) x R.dose x R.frequencia) */
            var result = dados.GroupBy(x => x.MedicamentoId ).Select(y => new EstoqueMedicamentosDashboardDTO
            {
                Medicamento = string.Format("{0} {1} {2}", y.First().Medicamento.Identificacao, y.First().Medicamento.Concentracao,  y.First().Medicamento.Unidade.Identificacao),
                Quantidade = (
                    _dbContext.CartaoControles
                        .Where(x => x.MedicamentoId == y.Key)
                        .OrderByDescending(x => x.Data)
                        .Select(x => x.Quantidade - ((DateTime.Now - x.Data).Value.Days * _dbContext.Receituarios
                                                                                                    .Where(z => z.MedicamentoId == y.Key)
                                                                                                    .Select(z => z.Dose * z.Frequencia)
                                                                                                    .FirstOrDefault()
                        ))
                        .FirstOrDefault()
                )
            }).ToList();

            // Retornar Zero caso o valor do estoque seja negativo
            var retorno = new List<EstoqueMedicamentosDashboardDTO>();
            foreach (var item in result)
            {
                retorno.Add(new EstoqueMedicamentosDashboardDTO
                {
                    Medicamento = item.Medicamento,
                    Quantidade = item.Quantidade < 0 ? 0 : item.Quantidade
                });
            }

            return retorno;
        }

    }
}