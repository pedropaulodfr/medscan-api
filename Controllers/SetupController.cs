using authentication_jwt.DTO;
using authentication_jwt.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace authentication_jwt.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SetupController : ControllerBase
    {
        private readonly SetupService _setupService;

        public SetupController(SetupService setupService)
        {
            _setupService = setupService;
        }

        [HttpGet]
        public async Task<ActionResult> Get()
        {
            try
            {
                return StatusCode(200, await _setupService.Get());
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
        
        [Authorize(Policy = "AdminPolicy")]
        [HttpPut]
        [Route("updateSMTP")]
        public async Task<ActionResult> UpdateSMTP([FromBody] SetupDTO model)
        {
            try
            {
                return StatusCode(200, await _setupService.UpdateSMTP(model));
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [Authorize(Policy = "AdminPolicy")]
        [HttpPut]
        [Route("update")]
        public async Task<ActionResult> Update([FromBody] SetupDTO model)
        {
            try
            {
                return StatusCode(200, await _setupService.Update(model));
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
