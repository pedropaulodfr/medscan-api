using authentication_jwt.DTO;
using authentication_jwt.Models;
using authentication_jwt.Services;
using Microsoft.AspNetCore.Mvc;

namespace authentication_jwt.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DashboardController : ControllerBase
    {
        private readonly DashboardService _dashboardService;

        public DashboardController(DashboardService dashboardService)
        {
            _dashboardService = dashboardService;
        }

        [HttpGet]
        [Route("cartaoControle/{PacienteId?}")]
        public async Task<ActionResult> CartaoControle(long PacienteId)
        {
            try
            {
                return StatusCode(200, await _dashboardService.CartaoControle(PacienteId));
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensagem = "Erro ao obter dados: " + ex });
            }
        }
        
        [HttpGet]
        [Route("proximoRetorno/{PacienteId?}")]
        public async Task<ActionResult> CardProximoAoRetorno(long PacienteId)
        {
            try
            {
                return StatusCode(200, await _dashboardService.CardProximoRetorno(PacienteId));
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensagem = "Erro ao obter dados: " + ex });
            }
        }
        
        [HttpGet]
        [Route("qntMedicamentosPaciente/{PacienteId?}")]
        public async Task<ActionResult> CardQntMedicamentosPaciente(long? PacienteId)
        {
            try
            {
                return StatusCode(200, await _dashboardService.CardQntMedicamentosPaciente(PacienteId));
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensagem = "Erro ao obter dados: " + ex });
            }
        }
        
        [HttpGet]
        [Route("estoqueMedicamentos/{PacienteId?}")]
        public async Task<ActionResult> EstoqueMedicamentos(long PacienteId)
        {
            try
            {
                return StatusCode(200, await _dashboardService.EstoqueMedicamentos(PacienteId));
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensagem = "Erro ao obter dados: " + ex });
            }
        }
    }
}
