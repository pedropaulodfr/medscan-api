using authentication_jwt.DTO;
using authentication_jwt.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace authentication_jwt.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UsuariosController : ControllerBase
    {
        private readonly UsuariosService _usuariosService;

        public UsuariosController(UsuariosService usuariosService)
        {
            _usuariosService = usuariosService;
        }

        [Authorize]
        [HttpGet]
        [Route("get/{id}")]
        public async Task<ActionResult> Get(long id)
        {
            try
            {
                return StatusCode(200,  await _usuariosService.Get(id));
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex });
            }
        }

        [Authorize(Policy = "AdminPolicy")]
        [HttpGet]
        [Route("getAll")]
        public async Task<ActionResult> GetAll()
        {
            try
            {
                return StatusCode(200, await _usuariosService.GetAll());
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex });
            }
        }

        [Authorize(Policy = "AdminPolicy")]
        [HttpPost]
        [Route("insert")]
        public async Task<ActionResult> Insert([FromBody] UsuarioDTO model)
        {
            try
            {
                return StatusCode(200, await _usuariosService.Insert(model));
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
        
        [Authorize(Policy = "AdminPolicy")]
        [HttpPut]
        [Route("update")]
        public async Task<ActionResult> Update([FromBody] UsuarioDTO model)
        {
            try
            {
                return StatusCode(200, await _usuariosService.Update(model));
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [Authorize]
        [HttpPut]
        [Route("updateImagem")]
        public async Task<ActionResult> UpdateImagem([FromBody] UsuarioDTO model)
        {
            try
            {
                return StatusCode(200, await _usuariosService.UpdateImagem(model));
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
        
        [Authorize(Policy = "AdminPolicy")]
        [HttpDelete]
        [Route("delete/{id}")]
        public async Task<ActionResult> Delete(long id)
        {
            await _usuariosService.Delete(id);  // Chama o método no serviço
            try
            {
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        } 
    }
}
