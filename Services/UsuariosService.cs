using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using authentication_jwt.DTO;
using authentication_jwt.Models;
using Microsoft.EntityFrameworkCore;

namespace authentication_jwt.Services
{
    public class UsuariosService
    {
        private readonly AppDbContext _dbContext;

        // Construtor para injetar o AppDbContext
        public UsuariosService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
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
                    Paciente = objPaciente
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
                Ativo = x.Ativo ? "Ativo" : "Inativo"
            }).ToList();

            return retorno;
        }
        public async Task<UsuarioDTO> Insert(UsuarioDTO model)
        {
            try
            {
                var existUsuario = await _dbContext.Usuarios.Where(x => x.Email == model.Email).FirstOrDefaultAsync();
                if(existUsuario != null)
                    throw new ArgumentException("Já existe um usuário cadastrado com esse e-mail!");

                Usuario usuario = new Usuario()
                {
                    Perfil = model.Perfil,
                    Nome = model.Nome,
                    Email = model.Email.Trim(),
                    ImagemPerfil = model.ImagemPerfil,
                    CodigoCadastro = model.CodigoCadastro,
                    Ativo = model.Ativo == "Ativo" ? true : false,
                    Senha = GerarSenhaAleatoria()
                };

                await _dbContext.AddAsync(usuario);
                await _dbContext.SaveChangesAsync();

                return model;
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

                var existUsuarioEmail = await _dbContext.Usuarios.Where(x => x.Email == model.Email.Trim() && x.Id != model.Id).FirstOrDefaultAsync();
                if (existUsuarioEmail != null)
                    throw new ArgumentException("Já existe um usuário cadastrado com esse e-mail!");

                existUsuario.Perfil = model.Perfil;
                existUsuario.Email = model.Email.Trim();
                existUsuario.CodigoCadastro = model.CodigoCadastro;
                existUsuario.Ativo = model.Ativo == "Ativo" ? true : false;

                if(!string.IsNullOrEmpty(model.ImagemPerfil))
                    existUsuario.ImagemPerfil = model.ImagemPerfil;
                if(!string.IsNullOrEmpty(model.Senha))
                    existUsuario.Senha = model.Senha;
                
                if (existUsuario.Perfil == "Paciente" && model.Ativo == "Inativo")
                    existUsuario.Pacientes.FirstOrDefault().Deletado = true;
                else if (existUsuario.Perfil == "Paciente" && model.Ativo == "Ativo")
                    existUsuario.Pacientes.FirstOrDefault().Deletado = false;
                
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


                // Gerar caminho do arquivo
                string hash = GerarHashSHA256(GerarSenhaAleatoria());
                string pastaImagens = Path.Combine(Directory.GetCurrentDirectory(), "Images");
                string arquivo = Path.Combine(pastaImagens, $"{hash}.png");

                // Criar diretório se não existir
                if (!Directory.Exists(pastaImagens))
                    Directory.CreateDirectory(pastaImagens);

                // Converter Base64 para bytes e salvar o arquivo
                byte[] imagemBytes = Convert.FromBase64String(model.ImagemPerfil);
                File.WriteAllBytes(arquivo, imagemBytes);
                
                existUsuairo.ImagemPerfil = $"{hash}.png";
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
                    throw new Exception("O usuário não existe!");

                if (usuario.Perfil == "Paciente")
                    usuario.Pacientes.FirstOrDefault().Deletado = true;

                usuario.Deletado = true;
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message ?? ex.InnerException.ToString());
            }
        }
        public string GerarSenhaAleatoria()
        {
            const string caracteres = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var random = new Random();
            return new string(Enumerable.Repeat(caracteres, 8)
                                        .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        static string GerarHashSHA256(string input)
    {
        using (SHA256 sha256 = SHA256.Create())
        {
            // Converter a string de entrada para bytes
            byte[] bytes = Encoding.UTF8.GetBytes(input);

            // Computar o hash
            byte[] hashBytes = sha256.ComputeHash(bytes);

            // Converter o hash para uma string hexadecimal
            StringBuilder hashHex = new StringBuilder();
            foreach (byte b in hashBytes)
            {
                hashHex.Append(b.ToString("x2")); // "x2" para formato hexadecimal com dois dígitos
            }

            return hashHex.ToString();
        }
    }

    }
}