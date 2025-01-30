using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using authentication_jwt.DTO;
using authentication_jwt.Models;
using authentication_jwt.Utils;
using Microsoft.EntityFrameworkCore;

namespace authentication_jwt.Services
{
    public class UsuariosService
    {
        private readonly AppDbContext _dbContext;
        private readonly EmailService _emailService;
        private readonly AcessoService _acessoService;

        // Construtor para injetar o AppDbContext
        public UsuariosService(AppDbContext dbContext, EmailService emailService, AcessoService acessoService)
        {
            _dbContext = dbContext;
            _emailService = emailService;
            _acessoService = acessoService; 
        }

        public async Task<UsuarioDTO> Get(long id)
        {
            try
            {
                Usuario usuario = await _dbContext.Usuarios.Where(x => x.Id == id).AsNoTracking().FirstOrDefaultAsync();

                PacienteDTO objPaciente = new PacienteDTO();
                if(usuario.Perfil == "Paciente")
                {
                    var paciente = await _dbContext.Pacientes.Where(x => x.UsuariosId == usuario.Id && x.Deletado != true).FirstOrDefaultAsync();

                    if (paciente == null)
                        throw new ArgumentException("Paciente não localizado!");

                    objPaciente = new PacienteDTO
                    {
                        Id = paciente.Id,
                        Nome = paciente.Nome,
                        NomeCompleto = paciente.NomeCompleto,
                        Email = paciente.Email,
                        Cpf = paciente.Cpf,
                        DataNascimento = paciente.DataNascimento,
                        Endereco = string.Format(@"{0}, {1}, {2}, {3}, {4}, {5}, {6}", paciente.Logradouro, paciente.Numero, paciente.Complemento, paciente.Bairro, paciente.Cidade, paciente.Uf, paciente.Cep ),
                        Logradouro = paciente.Logradouro,
                        Bairro = paciente.Bairro,
                        Complemento = paciente.Complemento,
                        Numero = paciente.Numero,
                        Cidade = paciente.Cidade,
                        Uf = paciente.Uf,
                        Cep = paciente.Cep,
                        Cns = paciente.Cns,
                        PlanoSaude = paciente.PlanoSaude,
                        UsuariosId = paciente.UsuariosId
                    };
                }

                var retorno = new UsuarioDTO
                {
                    Id = usuario.Id,
                    Perfil = usuario.Perfil,
                    Nome = usuario.Nome,
                    Email = usuario.Email,
                    ImagemPerfil = usuario.ImagemPerfil,
                    CodigoCadastro = usuario.CodigoCadastro,
                    Ativo = usuario.Ativo ? "Ativo" : "Inativo",
                    Paciente = objPaciente,
                    Master = usuario.Master
                };

                return retorno;
            }
            catch (Exception ex)
            {
                throw new ArgumentException(ex.Message ?? ex.InnerException.ToString());
            }
            
        }

        public async Task<List<UsuarioDTO>> GetAll()
        {
            List<Usuario> usuarios = await _dbContext.Usuarios.Where(x => x.Deletado != true).AsNoTracking().ToListAsync();

            var retorno = usuarios.Select(x => new UsuarioDTO
            {
                Id = x.Id,
                Perfil = x.Perfil,
                Nome = x.Nome,
                Email = x.Email,
                ImagemPerfil = x.ImagemPerfil,
                CodigoCadastro = x.CodigoCadastro,
                Ativo = x.Ativo ? "Ativo" : "Inativo",
                Master = x.Master
            }).ToList();

            return retorno;
        }
        public async Task<UsuarioDTO> Insert(UsuarioDTO model)
        {
            try
            {
                using (var transaction = _dbContext.Database.BeginTransaction())
                {
                    var existUsuario = await _dbContext.Usuarios.Where(x => x.Email == model.Email.Trim() && x.Deletado != true).FirstOrDefaultAsync();
                    if(existUsuario != null)
                        throw new ArgumentException("Já existe um usuário cadastrado com esse e-mail!");

                    if( model.Perfil == null && _acessoService.Perfil == "Admin")
                        model.Perfil = "Admin";
                    else 
                        model.Perfil = "Paciente";

                    Usuario usuario = new Usuario()
                    {
                        Perfil = model.Perfil,
                        Nome = model.Nome,
                        Email = model.Email.Trim(),
                        ImagemPerfil = model.ImagemPerfil,
                        CodigoCadastro = model.CodigoCadastro,
                        Ativo = model.Ativo == "Ativo" ? true : false,
                        Senha = Funcoes.GerarSenhaAleatoria(),
                        Master = model.Master
                    };
                    

                    await _dbContext.AddAsync(usuario);
                    await _dbContext.SaveChangesAsync();
                    await _emailService.EnviarEmailNovoCadastro(usuario.Email, usuario.Nome, usuario.Senha);

                    transaction.Commit();
                    
                    return model;
                }
            }
            catch (Exception ex)
            {
                throw new ArgumentException(ex.Message ?? ex.InnerException.ToString());
            }
        }
        public async Task<UsuarioDTO> Update(UsuarioDTO model)
        {
            try
            {
                var existUsuario = await _dbContext.Usuarios.Include(x => x.Pacientes).Where(u => u.Id == model.Id).FirstOrDefaultAsync();
                if (existUsuario == null)
                    throw new ArgumentException("Erro ao atualizar, o usuário não existe!");

                var existUsuarioEmail = await _dbContext.Usuarios.Where(x => x.Email == model.Email.Trim() && x.Id != model.Id && x.Deletado != true).FirstOrDefaultAsync();
                if (existUsuarioEmail != null)
                    throw new ArgumentException("Já existe um usuário cadastrado com esse e-mail!");

                existUsuario.Perfil = model.Perfil;
                existUsuario.Email = model.Email.Trim();
                existUsuario.CodigoCadastro = model.CodigoCadastro;
                existUsuario.Ativo = model.Ativo == "Ativo" ? true : false;
                existUsuario.Master = model.Master;

                if(!string.IsNullOrEmpty(model.ImagemPerfil))
                    existUsuario.ImagemPerfil = model.ImagemPerfil;
                if(!string.IsNullOrEmpty(model.Senha))
                    existUsuario.Senha = model.Senha;
                
                if (existUsuario.Perfil == "Paciente" && model.Ativo == "Inativo")
                    existUsuario.Pacientes.FirstOrDefault().Deletado = true;
                else if (existUsuario.Perfil == "Paciente" && model.Ativo == "Ativo")
                {
                    var existPaciente = await _dbContext.Pacientes.Where(x => x.Email == model.Email && x.UsuariosId != existUsuario.Id && x.Deletado == false).FirstOrDefaultAsync();
                    if(existPaciente != null)
                        throw new ArgumentException("Já existe um paciente cadastrado com esse E-mail ou CPF!");
                    existUsuario.Pacientes.FirstOrDefault().Deletado = false;
                }
                
                await _dbContext.SaveChangesAsync();

                return model;
            }
            catch (Exception ex)
            {
                throw new ArgumentException(ex.Message ?? ex.InnerException.ToString());
            }
        }

        public async Task<UsuarioDTO> UpdateImagem(UsuarioDTO model)
        {
            try
            {
                var existUsuairo = await _dbContext.Usuarios.Where(x => x.Id == model.Id && x.Ativo).FirstOrDefaultAsync();
                if (existUsuairo == null)
                    throw new ArgumentException("Usuário não localizado!");

                var funcoes = new Funcoes();
                var resultado = await funcoes.UploadImagem(model.ImagemPerfil);

                if(string.IsNullOrEmpty(resultado))
                    throw new ArgumentException("Erro ao fazer upload da imagem!");

                existUsuairo.ImagemPerfil = resultado;
                await _dbContext.SaveChangesAsync();

                return model;
            }
            catch (Exception ex)
            {
                throw new ArgumentException(ex.Message ?? ex.InnerException.ToString());
            }
        }
        
        public async Task Delete(long id)
        {
            try
            {
                var usuario = await _dbContext.Usuarios.Include(x => x.Pacientes).Where(u => u.Id == id).FirstOrDefaultAsync();
                if (usuario == null)
                    throw new ArgumentException("O usuário não existe!");

                if (usuario.Perfil == "Paciente")
                    usuario.Pacientes.FirstOrDefault().Deletado = true;

                usuario.Deletado = true;
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new ArgumentException(ex.Message ?? ex.InnerException.ToString());
            }
        }

        public async Task EsqueceuSenha(string email)
        {
            try
            {
                var usuario = await _dbContext.Usuarios.Where(x => x.Email == email).FirstOrDefaultAsync();
                if (usuario == null)
                    throw new ArgumentException("Usuário não localizado!");

                //usuario.Senha = Funcoes.GerarSenhaAleatoria();
                //await _dbContext.SaveChangesAsync();

                await _emailService.EnviarEmailEsqueceuSenha(usuario.Email, usuario.Nome, usuario.Senha);
            }
            catch (Exception ex)
            {
                throw new ArgumentException(ex.Message ?? ex.InnerException.ToString());
            }
        }
    }
}