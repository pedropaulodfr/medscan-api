using System.Collections.Generic;
using authentication_jwt.DTO;
using authentication_jwt.Models;
using Microsoft.EntityFrameworkCore;

namespace authentication_jwt.Services
{
    public class TipoMedicamentoService
    {
        private readonly AppDbContext _dbContext;

        // Construtor para injetar o AppDbContext
        public TipoMedicamentoService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        // Método para retornar todos os tipos de medicamentos
        public async Task<List<TipoMedicamento>> GetAll()
        {
            return await _dbContext.TipoMedicamentos.ToListAsync();
        }

        public async Task<TipoMedicamentosDTO> Insert(TipoMedicamentosDTO model)
        {
            try
            {
                if (model.Id != 0)
                    throw new Exception("Erro ao salvar, o tipo de medicamento já existe!");
                
                TipoMedicamento tipoMedicamento = new TipoMedicamento()
                {
                    Identificacao = model.Identificacao,
                    Descricao = model.Descricao ?? "",
                };

                await _dbContext.AddAsync(tipoMedicamento);
                await _dbContext.SaveChangesAsync();

                return model;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message ?? ex.InnerException.ToString());
            }
        }
        public async Task<TipoMedicamentosDTO> Update(TipoMedicamentosDTO model)
        {
            try
            {
                var existTipoMedicamento = await _dbContext.TipoMedicamentos.Where(x => x.Id == model.Id).FirstOrDefaultAsync();
                if (existTipoMedicamento == null)
                    throw new Exception("Erro ao atualizar, o tipo de medicamento não existe!");
                
                existTipoMedicamento.Identificacao = model.Identificacao;
                existTipoMedicamento.Descricao = model.Descricao;
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
                var tipoMedicamento = await _dbContext.TipoMedicamentos.Where(x => x.Id == id).FirstOrDefaultAsync();
                if (tipoMedicamento == null)
                    throw new Exception("Erro ao deletar, o tipo de medicamento não existe!");

                _dbContext.Remove(tipoMedicamento);
                await _dbContext.SaveChangesAsync();

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message ?? ex.InnerException.ToString());
            }
        }
    }
}
