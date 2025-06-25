using authentication_jwt.DTO;
using authentication_jwt.DTO.Relatorios;
using authentication_jwt.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace authentication_jwt.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RelatoriosController : ControllerBase
    {
        private readonly RelatoriosService _relatoriosService;

        public RelatoriosController(RelatoriosService relatoriosService)
        {
            _relatoriosService = relatoriosService;
        }

        [Authorize]
        [HttpPost]
        [Route("relatorioMedicamentos")]
        public async Task<ActionResult> RelatorioMedicamentos(RelatorioMedicamentosFiltros model)
        {
            try
            {
                return StatusCode(200, await _relatoriosService.RelatorioMedicamentos(model));
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex });
            }
        }
    }
}