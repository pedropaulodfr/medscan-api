using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using authentication_jwt.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace authentication_jwt.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class NotificacoesController : ControllerBase
    {
        private readonly NotificacoesService _notificacoesService;

        public NotificacoesController(NotificacoesService notificacoesService)
        {
            _notificacoesService = notificacoesService;
        }

        [HttpGet]
        [Route("criarNotificacao")]
        public async Task<ActionResult> CriarNotificacao()
        {
            try
            {
                await _notificacoesService.CriarNotificacoes();
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex });
            }
        }

        [HttpGet]
        [Route("getUsuariosNotificacoes")]
        public async Task<ActionResult> GetUsuariosNotificacoes()
        {
            try
            {
                return StatusCode(200, await _notificacoesService.GetUsuariosNotificacoes());
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex });
            }
        }

        [HttpGet]
        [Route("processarNotificacoes")]
        public async Task<ActionResult> ProcessarNotificacoes()
        {
            try
            {
                Console.WriteLine("Iniciando processamento de notificações...");
                await _notificacoesService.ProcessarNotificacoes();
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex });
            }
        }
    }
}