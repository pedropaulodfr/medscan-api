using authentication_jwt.DTO;
using authentication_jwt.Models;
using Microsoft.EntityFrameworkCore;

namespace authentication_jwt.Services
{
    public class MedicamentosService
    {
        private readonly AppDbContext _dbContext;
        private readonly AcessoService _acesso;

        // Construtor para injetar o AppDbContext
        public MedicamentosService(AppDbContext dbContext, AcessoService acesso)
        {
            _dbContext = dbContext;
            _acesso = acesso;
        }

        public async Task<List<MedicamentoDTO>> GetAll()
        {
            List<Medicamento> medicamentos = new List<Medicamento>();
            
            if(_acesso.Perfil == "Admin")
                medicamentos = await _dbContext.Medicamentos.AsNoTracking().ToListAsync();
            else
                medicamentos = await _dbContext.Medicamentos.Where(x => x.Inativo != true).AsNoTracking().ToListAsync();

            List<MedicamentoDTO> retorno = medicamentos.Select(m => new MedicamentoDTO
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

            return retorno;
        }

        public async Task<Medicamento> Insert(MedicamentoDTO model)
        {
            try
            {
                if (model.Id != 0)
                    throw new Exception("Erro ao salvar, o medicamento já existe!");

                Medicamento medicamento = new Medicamento()
                {
                    Identificacao = model.Identificacao,
                    Descricao = model.Descricao ?? "",
                    TipoMedicamentoId = model.TipoMedicamentoId.Value,
                    Concentracao = model.Concentracao,
                    UnidadeId = model.UnidadeId,
                    Associacao = model.Associacao,
                    Inativo = model.Status == "Inativo" ? true : false,
                };

                await _dbContext.AddAsync(medicamento);
                await _dbContext.SaveChangesAsync();

                return medicamento;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message ?? ex.InnerException.ToString());
            }
        }

        public async Task<MedicamentoDTO> Update(MedicamentoDTO model)
        {
            try
            {
                var existMedicamento = await _dbContext.Medicamentos.Where(m => m.Id == model.Id).FirstOrDefaultAsync();
                if (existMedicamento == null)
                    throw new Exception("Erro ao atualizar, o medicamento não existe!");
                
                existMedicamento.Identificacao = model.Identificacao;
                existMedicamento.Descricao = model.Descricao;
                existMedicamento.Concentracao = model.Concentracao;
                existMedicamento.TipoMedicamentoId = model.TipoMedicamentoId.Value;
                existMedicamento.UnidadeId = model.UnidadeId;
                existMedicamento.Associacao = model.Associacao;
                existMedicamento.Inativo = model.Status == "Inativo" ? true : false;

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
            var medicamento = await _dbContext.Medicamentos.Where(m => m.Id == id).FirstOrDefaultAsync();
            if (medicamento == null)
                throw new Exception("Erro ao deletar, o medicamento não existe!");

            _dbContext.Remove(medicamento);
            await _dbContext.SaveChangesAsync();
        }
    }
}