using authentication_jwt.DTO;
using authentication_jwt.Services;
using Microsoft.AspNetCore.Mvc;

namespace authentication_jwt.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ReceituarioController : ControllerBase
    {
        private readonly ReceituarioService _receituarioService;

        public ReceituarioController(ReceituarioService receituarioService)
        {
            _receituarioService = receituarioService;
        }

        [HttpGet]
        [Route("get/{PacienteId}")]
        public async Task<ActionResult> Get(long PacienteId)
        {
            try
            {
                return StatusCode(200, await _receituarioService.Get(PacienteId));
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensagem = "Erro ao obter dados: " + ex });
            }
        }

        [HttpGet]
        [Route("getAll")]
        public async Task<ActionResult> GetAll()
        {
            try
            {
                return StatusCode(200, await _receituarioService.GetAll());
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensagem = "Erro ao obter dados: " + ex });
            }
        }

        [HttpPost]
        [Route("insert")]
        public async Task<ActionResult> Insert([FromBody] ReceituarioDTO model)
        {
            try
            {
                return StatusCode(200, await _receituarioService.Insert(model));
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensagem = "Erro ao salvar: " + ex });
            }
        }
        
        [HttpPut]
        [Route("update")]
        public async Task<ActionResult> Update([FromBody] ReceituarioDTO model)
        {
            try
            {
                return StatusCode(200, await _receituarioService.Update(model));
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensagem = "Erro ao atualizar: " + ex });
            }
        }
        
        [HttpDelete]
        [Route("delete/{id}")]
        public async Task<ActionResult> Delete(long id)
        {
            await _receituarioService.Delete(id);  // Chama o método no serviço
            try
            {
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensagem = "Erro ao deletar: " + ex });
            }
        }
    }
}
