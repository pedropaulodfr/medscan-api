using authentication_jwt.DTO;
using authentication_jwt.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace authentication_jwt.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PacientesController : ControllerBase
    {
        private readonly PacientesService _pacientesService;

        public PacientesController(PacientesService pacientesService)
        {
            _pacientesService = pacientesService;
        }

        [Authorize]
        [HttpGet]
        [Route("get/{Id}")]
        public async Task<ActionResult> Get(long Id)
        {
            try
            {
                return StatusCode(200,  await _pacientesService.Get(Id));
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
                return StatusCode(200, await _pacientesService.GetAll());
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex });
            }
        }

        [Authorize(Policy = "AdminPolicy")]
        [HttpPost]
        [Route("insert")]
        public async Task<ActionResult> Insert([FromBody] PacienteDTO model)
        {
            try
            {
                return StatusCode(200, await _pacientesService.Insert(model));
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
        
        //[Authorize(Policy = "AdminPolicy")]
        [Authorize]
        [HttpPut]
        [Route("update")]
        public async Task<ActionResult> Update([FromBody] PacienteDTO model)
        {
            try
            {
                return StatusCode(200, await _pacientesService.Update(model));
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
            await _pacientesService.Delete(id);  // Chama o método no serviço
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
