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
        [Route("cartaoControle/{PacienteId}")]
        public async Task<ActionResult> CartaoControle(long PacienteId)
        {
            var result = await _dashboardService.CartaoControle(PacienteId);  // Chama o método no serviço

            try
            {
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensagem = "Erro ao obter dados: " + ex });
            }
        }
        
        [HttpGet]
        [Route("proximoRetorno/{PacienteId}")]
        public async Task<ActionResult> CardProximoAoRetorno(long PacienteId)
        {
            var result = await _dashboardService.CardProximoRetorno(PacienteId);  // Chama o método no serviço

            try
            {
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensagem = "Erro ao obter dados: " + ex });
            }
        }
        
        [HttpGet]
        [Route("estoqueMedicamentos/{PacienteId}")]
        public async Task<ActionResult> EstoqueMedicamentos(long PacienteId)
        {
            var result = await _dashboardService.EstoqueMedicamentos(PacienteId);  // Chama o método no serviço

            try
            {
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensagem = "Erro ao obter dados: " + ex });
            }
        }
    }
}
