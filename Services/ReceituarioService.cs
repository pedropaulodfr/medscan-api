using authentication_jwt.DTO;
using authentication_jwt.Models;
using Microsoft.EntityFrameworkCore;

namespace authentication_jwt.Services
{
    public class ReceituarioService
    {
        private readonly AppDbContext _dbContext;

        // Construtor para injetar o AppDbContext
        public ReceituarioService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<ReceituarioDTO>> Get(long PacienteId)
        {
            var receituarios = await _dbContext.Receituarios
                                                .Include(x => x.TipoMedicamento)
                                                    .ThenInclude(y => y.Medicamentos)
                                                        .ThenInclude(z => z.Unidade)
                                                .Where(x => x.PacienteId == PacienteId)
                                                .AsNoTracking().ToListAsync();

            try
            {
                List<ReceituarioDTO> retorno = receituarios.Select(m => new ReceituarioDTO
                {
                    Id = m.Id,
                    Frequencia = m.Frequencia,
                    Tempo = m.Tempo,
                    Periodo = m.Periodo,
                    Dose = m.Dose,
                    Medicamento = new MedicamentoDTO 
                    {
                        Id = m.MedicamentoId,
                        Identificacao = m.TipoMedicamento.Medicamentos.Where(x => x.Id == m.MedicamentoId).FirstOrDefault()?.Identificacao ?? "Sem Identificação",
                        Descricao = m.TipoMedicamento.Medicamentos.Where(x => x.Id == m.MedicamentoId).FirstOrDefault()?.Descricao ?? "",
                        Concentracao = m.TipoMedicamento.Medicamentos.Where(x => x.Id == m.MedicamentoId).FirstOrDefault()?.Concentracao.ToString() ?? "Sem Concentração",
                        Unidade = m.TipoMedicamento.Medicamentos.Where(x => x.Id == m.MedicamentoId).FirstOrDefault()?.Unidade?.Identificacao ?? "Sem Unidade",
                        TipoMedicamento = m.TipoMedicamento.Identificacao,
                        TipoMedicamentoId = m.TipoMedicamentoId,
                    }
                }).ToList();

                return retorno;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message ?? ex.InnerException.ToString());
            }
        }
        public async Task<List<ReceituarioDTO>> GetAll()
        {
            var receituarios = await _dbContext.Receituarios
                                                .Include(x => x.TipoMedicamento)
                                                    .ThenInclude(y => y.Medicamentos)
                                                        .ThenInclude(z => z.Unidade)
                                                .AsNoTracking()
                                                .ToListAsync();

            try
            {
                List<ReceituarioDTO> retorno = receituarios.Select(m => new ReceituarioDTO
                {
                    Id = m.Id,
                    Frequencia = m.Frequencia,
                    Tempo = m.Tempo,
                    Periodo = m.Periodo,
                    Dose = m.Dose,
                    Medicamento = new MedicamentoDTO 
                    {
                        Id = m.MedicamentoId,
                        Identificacao = m.TipoMedicamento.Medicamentos.Where(x => x.Id == m.MedicamentoId).FirstOrDefault()?.Identificacao ?? "Sem Identificação",
                        Descricao = m.TipoMedicamento.Medicamentos.Where(x => x.Id == m.MedicamentoId).FirstOrDefault()?.Descricao ?? "",
                        Concentracao = m.TipoMedicamento.Medicamentos.Where(x => x.Id == m.MedicamentoId).FirstOrDefault()?.Concentracao.ToString() ?? "Sem Concentração",
                        Unidade = m.TipoMedicamento.Medicamentos.Where(x => x.Id == m.MedicamentoId).FirstOrDefault()?.Unidade?.Identificacao ?? "Sem Unidade",
                        TipoMedicamento = m.TipoMedicamento.Identificacao,
                        TipoMedicamentoId = m.TipoMedicamentoId,
                    }
                }).ToList();

                return retorno;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message ?? ex.InnerException.ToString());
            }
        }

        public async Task<ReceituarioDTO> Insert(ReceituarioDTO model)
        {
            try
            {
                var paciente = await _dbContext.Pacientes.Where(x => x.UsuariosId == model.UsuarioId).FirstOrDefaultAsync();
                if (paciente == null)
                    throw new Exception("Paciente não encontrado!");

                Receituario receituario = new Receituario()
                {
                    Frequencia = model.Frequencia,
                    Tempo = model.Tempo,
                    Periodo = model.Periodo,
                    Dose = model.Dose,
                    MedicamentoId = model.Medicamento.Id,
                    TipoMedicamentoId = model.Medicamento.TipoMedicamentoId.Value,
                    PacienteId = paciente.Id
                };

                await _dbContext.AddAsync(receituario);
                await _dbContext.SaveChangesAsync();

                return model;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message ?? ex.InnerException.ToString());
            }
        }

        public async Task<ReceituarioDTO> Update(ReceituarioDTO model)
        {
            try
            {
                var existReceituario = await _dbContext.Receituarios.Where(x => x.Id == model.Id).FirstOrDefaultAsync();
                if (existReceituario == null)
                    throw new Exception("Erro ao atualizar, o receituário não existe!");
                
                existReceituario.Frequencia = model.Frequencia;
                existReceituario.Tempo = model.Tempo;
                existReceituario.Periodo = model.Periodo;
                existReceituario.Dose = model.Dose;
                existReceituario.MedicamentoId = model.Medicamento.Id;
                existReceituario.TipoMedicamentoId = model.Medicamento.TipoMedicamentoId.Value;
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
                var receituario = await _dbContext.Receituarios.Where(x => x.Id == id).FirstOrDefaultAsync();
                if (receituario == null)
                    throw new Exception("Erro ao deletar, o receituário não existe!");

                _dbContext.Remove(receituario);
                await _dbContext.SaveChangesAsync();

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message ?? ex.InnerException.ToString());
            }
        }
    }
}