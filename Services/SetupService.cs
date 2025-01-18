using System.Collections.Generic;
using authentication_jwt.DTO;
using authentication_jwt.Models;
using Microsoft.EntityFrameworkCore;

namespace authentication_jwt.Services
{
    public class SetupService
    {
        private readonly AppDbContext _dbContext;

        public SetupService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<SetupDTO> Get()
        {
            return await _dbContext.Setups.Select(x => new SetupDTO
            {
                Urlapi = x.Urlapi,
                CaminhoArquivos = x.CaminhoArquivos,
                UsarCodigoCadastro = x.UsarCodigoCadastro,
                SmtpHost = x.SmtpHost,
                SmtpPort = x.SmtpPort,
                SmtpUser = x.SmtpUser,
                SmtpPassword = x.SmtpPassword
            }).FirstOrDefaultAsync();
        }

        public async Task<SetupDTO> Update(SetupDTO model)
        {
            try
            {
                return model;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message ?? ex.InnerException.ToString());
            }
        }
    }
}
