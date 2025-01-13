using System.Collections.Generic;
using authentication_jwt.DTO;
using authentication_jwt.Models;
using Microsoft.EntityFrameworkCore;

namespace authentication_jwt.Services
{
    public class UnidadesService
    {
        private readonly AppDbContext _dbContext;

        // Construtor para injetar o AppDbContext
        public UnidadesService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        // Método para retornar todos os tipos de medicamentos
        public async Task<List<Unidade>> GetAll()
        {
            return await _dbContext.Unidades.ToListAsync();
        }

        public async Task<UnidadeDTO> Insert(UnidadeDTO model)
        {
            try
            {
                if (model.Id != 0)
                    throw new Exception("Erro ao salvar, a unidade já existe!");
                
                Unidade unidade = new Unidade()
                {
                    Identificacao = model.Identificacao,
                    Descricao = model.Descricao ?? "",
                };

                await _dbContext.AddAsync(unidade);
                await _dbContext.SaveChangesAsync();

                return model;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message ?? ex.InnerException.ToString());
            }
        }
        public async Task<UnidadeDTO> Update(UnidadeDTO model)
        {
            try
            {
                var existUnidade = await _dbContext.Unidades.Where(u => u.Id == model.Id).FirstOrDefaultAsync();
                if (existUnidade == null)
                    throw new Exception("Erro ao atualizar, a unidade não existe!");
                
                existUnidade.Identificacao = model.Identificacao;
                existUnidade.Descricao = model.Descricao;
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
                var unidade = await _dbContext.Unidades.Where(u => u.Id == id).FirstOrDefaultAsync();
                if (unidade == null)
                    throw new Exception("Erro ao deletar, a unidade não existe!");

                _dbContext.Remove(unidade);
                await _dbContext.SaveChangesAsync();

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message ?? ex.InnerException.ToString());
            }
        }

    }
}
