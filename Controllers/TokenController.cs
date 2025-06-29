using authentication_jwt.DTO;
using authentication_jwt.Models;
using authentication_jwt.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Core;
using Microsoft.EntityFrameworkCore;

namespace authentication_jwt.Controllers
{
    public class AuthController : ControllerBase
    {

        private readonly AppDbContext _dbContext;

        public AuthController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpPost]
        [Route("login")]
        public async Task<ActionResult<dynamic>> Authenticate([FromBody] UsuarioDTO model)
        {
            try
            {
                // Recupera o usuário
                var user = await _dbContext.Usuarios.Where(x => x.Email == model.Email && x.Senha == model.Senha && x.Ativo != false && x.Deletado != true)
                .AsNoTracking()
                .FirstOrDefaultAsync();

                UsuarioDTO _user = new UsuarioDTO()
                {
                    Id = user.Id,
                    Nome = user.Nome,
                    Perfil = user.Perfil,
                    Master = user.Master,
                    Email = user.Email,
                    Senha = user.Senha,
                    PacienteId = 0
                };

                // Verifica se o usuário existe
                if (user == null)
                    return NotFound(new { message = "Usuário ou senha inválidos" });

                // Pegar id caso o usuário seja paciente
                if (user.Perfil == "Paciente")
                {
                    var paciente = await _dbContext.Pacientes.Where(x => x.UsuariosId == user.Id).AsNoTracking().FirstOrDefaultAsync();
                    if (paciente != null)
                        _user.PacienteId = paciente.Id;
                }
                
                // Gera o Token
                var token = TokenService.GenerateToken(_user);

                // Oculta a senha
                user.Senha = "";
                user.Email = model.Email;


                // Retorna os dados encapsulados em um ActionResult
                UsuarioAutenticadoDTO usuarioAutenticado = new UsuarioAutenticadoDTO
                {
                    UsuarioId = _user.Id.Value,
                    Nome = _user.Nome,
                    Email = _user.Email,
                    Perfil = _user.Perfil,
                    Master = _user.Master.ToString(),
                    Token = token,
                    PacienteId = _user.PacienteId
                };
                
                return Ok(usuarioAutenticado);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [HttpPost("validate")]
        public async Task<ActionResult<UserDTO>> ValidarToken([FromBody] UserDTO model)
        {
            // Lógica para validar o token JWT
            UserDTO user = TokenService.ValidarTokenJWT(model.Token);

            if (user != null)
            {
                return Ok(user);
            }
            else
            {
                return BadRequest(new { mensagem = "Token inválido" });
            }
        }

    }
}