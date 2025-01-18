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

        public async Task<SetupDTO> UpdateSMTP(SetupDTO model)
        {
            try
            {
                var setup = await _dbContext.Setups.FirstOrDefaultAsync();
                if (setup == null)
                    throw new ArgumentException("Setup não encontrado!");

                setup.SmtpHost = model.SmtpHost;
                setup.SmtpPort = model.SmtpPort;
                setup.SmtpUser = model.SmtpUser;
                setup.SmtpPassword = model.SmtpPassword;

                await _dbContext.SaveChangesAsync();

                return model;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message ?? ex.InnerException.ToString());
            }
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
