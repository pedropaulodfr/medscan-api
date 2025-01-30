using authentication_jwt.DTO;
using authentication_jwt.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace authentication_jwt.Controllers
{
    [Route("[controller]")]
    public class MedicamentosController : Controller
    {
        private readonly MedicamentosService _medicamentosService;

        public MedicamentosController(MedicamentosService medicamentosService)
        {
            _medicamentosService = medicamentosService;
        }

        [Authorize]
        [HttpGet]
        [Route("getAll")]
        public async Task<ActionResult> GetAll()
        {
            try
            {
                return StatusCode(200, await _medicamentosService.GetAll());
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex });
            }
        }
        
        [Authorize]
        [HttpGet]
        [Route("getMedicamentosReceituario/{PacienteId?}")]
        public async Task<ActionResult> GetMedicamentosReceituario(long? PacienteId)
        {
            try
            {
                return StatusCode(200, await _medicamentosService.GetMedicamentosReceituario(PacienteId.Value));
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex });
            }
        }
        
        [Authorize(Policy = "AdminPolicy")]
        [HttpPost]
        [Route("insert")]
        public async Task<ActionResult> Insert([FromBody] MedicamentoDTO model)
        {
            try
            {
                return StatusCode(200, await _medicamentosService.Insert(model));
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex });
            }
        }

        [Authorize(Policy = "AdminPolicy")]
        [HttpPut]
        [Route("update")]
        public async Task<ActionResult> Update([FromBody] MedicamentoDTO model)
        {
            try
            {
                return StatusCode(200,  await _medicamentosService.Update(model));
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex });
            }
        }
        
        [Authorize(Policy = "AdminPolicy")]
        [HttpDelete]
        [Route("delete/{id}")]
        public async Task<ActionResult> Delete(long id)
        {
            try
            {
                await _medicamentosService.Delete(id); 
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex });
            }
        }
    }
}