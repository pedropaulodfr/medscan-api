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

        [Authorize]
        [HttpGet]
        [Route("getAllNotificacoes")]
        public async Task<ActionResult> GetNotificaGetAllNotificacoescoesNaoLidas()
        {
            try
            {
                return StatusCode(200, await _notificacoesService.GetAllNotificacoes());
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex });
            }
        }

        [Authorize]
        [HttpGet]
        [Route("updateNotificacaoLida/{notificacaoId}")]
        public async Task<ActionResult> UpdateNotificacaoLida(long notificacaoId)
        {
            try
            {
                await _notificacoesService.UpdateNotificacaoLida(notificacaoId);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex });
            }
        }
    }
}