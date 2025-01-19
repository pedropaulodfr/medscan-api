using authentication_jwt.DTO;
using authentication_jwt.Services;
using Microsoft.AspNetCore.Authorization;
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

        [Authorize]
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
                return BadRequest(new { message = ex.Message });
            }
        }

        [Authorize(Policy = "AdminPolicy")]
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
                return BadRequest(new { message = ex.Message });
            }
        }

        [Authorize]
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
                return BadRequest(new { message = ex.Message });
            }
        }
        
        [Authorize]
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
                return BadRequest(new { message = ex.Message });
            }
        }
        
        [Authorize]
        [HttpDelete]
        [Route("delete/{id}")]
        public async Task<ActionResult> Delete(long id)
        {
            await _receituarioService.Delete(id); 
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
