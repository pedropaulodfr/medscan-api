using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using authentication_jwt.DTO;
using authentication_jwt.Models;
using Microsoft.EntityFrameworkCore;

namespace authentication_jwt.Services
{
    public class PacientesService
    {
        private readonly AppDbContext _dbContext;
        private readonly UsuariosService _usuariosService;

        // Construtor para injetar o AppDbContext
        public PacientesService(AppDbContext dbContext, UsuariosService usuariosService)
        {
            _dbContext = dbContext;
            _usuariosService = usuariosService;
        }

        public async Task<PacienteDTO> Get(long Id)
        {
            try
            {
                var paciente = await _dbContext.Pacientes.Where(x => x.UsuariosId == Id && x.Deletado != true).FirstOrDefaultAsync();

                if (paciente == null)
                    throw new ArgumentException("Paciente não localizado!");

                var retorno = new PacienteDTO
                {
                    Id = paciente.Id,
                    Nome = paciente.Nome,
                    NomeCompleto = paciente.NomeCompleto,
                    Email = paciente.Email,
                    Cpf = paciente.Cpf,
                    DataNascimento = paciente.DataNascimento,
                    Logradouro = paciente.Logradouro,
                    Bairro = paciente.Bairro,
                    Complemento = paciente.Complemento,
                    Numero = paciente.Numero,
                    Uf = paciente.Uf,
                    Cep = paciente.Cep,
                    Cns = paciente.Cns,
                    PlanoSaude = paciente.PlanoSaude,
                    UsuariosId = paciente.UsuariosId
                };

                return retorno;
            }
            catch (Exception ex)
            {
                throw new ArgumentException(ex.Message ?? ex.InnerException.ToString());
            }
            
        }

        public async Task<List<PacienteDTO>> GetAll()
        {
            List<Paciente> pacientes = await _dbContext.Pacientes.Include(x => x.Usuarios).Where(x => x.Deletado != true).AsNoTracking().ToListAsync();

            var retorno = pacientes.Select(x => new PacienteDTO
            {
                Id = x.Id,
                Nome = x.Nome,
                NomeCompleto = x.NomeCompleto,
                Email = x.Email,
                Cpf = x.Cpf,
                DataNascimento = x.DataNascimento,
                Logradouro = x.Logradouro,
                Bairro = x.Bairro,
                Complemento = x.Complemento,
                Numero = x.Numero,
                Cidade = x.Cidade,
                Uf = x.Uf,
                Cep = x.Cep,
                Endereco = string.Format(@"{0}, {1}, {2}, {3}, {4}, {5}, {6}", x.Logradouro, x.Numero, x.Complemento, x.Bairro, x.Cidade, x.Uf, x.Cep ),
                Cns = x.Cns,
                PlanoSaude = x.PlanoSaude,
                UsuariosId = x.UsuariosId,
                Usuarios = new UsuarioDTO
                {
                    Id = x.Usuarios.Id,
                    Perfil = x.Usuarios.Perfil,
                    Nome = x.Usuarios.Nome,
                    Email = x.Usuarios.Email,
                    ImagemPerfil = x.Usuarios.ImagemPerfil,
                    CodigoCadastro = x.Usuarios.CodigoCadastro,
                    Ativo = x.Usuarios.Ativo ? "Ativo" : "Inativo",
                    Senha = x.Usuarios.Senha,
                }
            }).ToList();

            return retorno;
        }
        public async Task<PacienteDTO> Insert(PacienteDTO model)
        {
            try
            {
                var existPaciente = await _dbContext.Pacientes.Where(x => x.Email == model.Email.Trim() || x.Cpf == model.Cpf).FirstOrDefaultAsync();
                if(existPaciente != null)
                    throw new ArgumentException("Já existe um paciente cadastrado com esse E-mail ou CPF!");

                Usuario usuario = new Usuario()
                {
                    Perfil = "Paciente",
                    Nome = model.Nome,
                    Email = model.Email.Trim(),
                    Ativo = true,
                    Senha = _usuariosService.GerarSenhaAleatoria()
                };

                Paciente paciente = new Paciente()    
                {
                    Id = model.Id,
                    Nome = model.Nome,
                    NomeCompleto = model.NomeCompleto,
                    Email = model.Email,
                    Cpf = model.Cpf.Replace(".", "").Replace("-", ""),
                    DataNascimento = model.DataNascimento,
                    Logradouro = model.Logradouro,
                    Bairro = model.Bairro,
                    Complemento = model.Complemento,
                    Numero = model.Numero,
                    Cidade = model.Cidade,
                    Uf = model.Uf,
                    Cep = model.Cep,
                    Cns = model.Cns,
                    PlanoSaude = model.PlanoSaude,
                    UsuariosId = model.UsuariosId,
                    Usuarios = usuario
                };

                await _dbContext.AddAsync(paciente);
                await _dbContext.SaveChangesAsync();

                return model;
            }
            catch (Exception ex)
            {
                throw new ArgumentException(ex.Message ?? ex.InnerException.ToString());
            }
        }
        public async Task<PacienteDTO> Update(PacienteDTO model)
        {
            try
            {
                var existPaciente = await _dbContext.Pacientes.Include(x => x.Usuarios).Where(u => u.Id == model.Id).FirstOrDefaultAsync();
                if (existPaciente == null)
                    throw new ArgumentException("Paciente não localizado!");

                var existPacienteEmailCPF = await _dbContext.Pacientes.Where(x => (x.Email == model.Email.Trim() || x.Cpf == model.Cpf) && x.Id != model.Id ).FirstOrDefaultAsync();
                if (existPacienteEmailCPF != null)
                    throw new ArgumentException("Já existe um paciente cadastrado com esse E-mail ou CPF!");

                existPaciente.Nome = model.Nome;
                existPaciente.NomeCompleto = model.NomeCompleto;
                existPaciente.Email = model.Email;
                existPaciente.Cpf = model.Cpf.Replace(".", "").Replace("-", "");
                existPaciente.DataNascimento = model.DataNascimento;
                existPaciente.Logradouro = model.Logradouro;
                existPaciente.Bairro = model.Bairro;
                existPaciente.Complemento = model.Complemento;
                existPaciente.Numero = model.Numero;
                existPaciente.Cidade = model.Cidade;
                existPaciente.Uf = model.Uf;
                existPaciente.Cep = model.Cep;
                existPaciente.Cns = model.Cns;
                existPaciente.PlanoSaude = model.PlanoSaude;
                existPaciente.Usuarios.Nome = model.Nome;
                existPaciente.Usuarios.Email = model.Email;
                if (!string.IsNullOrEmpty(model.Usuarios.Senha))
                    existPaciente.Usuarios.Senha = model.Usuarios.Senha;
                
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
                var paciente = await _dbContext.Pacientes.Include(x => x.Usuarios).Where(u => u.Id == id).FirstOrDefaultAsync();
                if (paciente == null)
                    throw new Exception("O paciente não existe!");

                paciente.Deletado = true;
                paciente.Usuarios.Ativo = false;

                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message ?? ex.InnerException.ToString());
            }
        }
    }
}