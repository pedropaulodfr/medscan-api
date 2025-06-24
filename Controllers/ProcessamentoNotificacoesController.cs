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
    public class ProcessamentoNotificacoesController : ControllerBase
    {
        private readonly ProcessamentoNotificacoesService _processamentoNotificacoesService;

        public ProcessamentoNotificacoesController(ProcessamentoNotificacoesService processamentoNotificacoesService)
        {
            _processamentoNotificacoesService = processamentoNotificacoesService;
        }

        [HttpGet]
        [Route("criarNotificacao")]
        public async Task<ActionResult> CriarNotificacao()
        {
            try
            {
                await _processamentoNotificacoesService.CriarNotificacoes();
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
                return StatusCode(200, await _processamentoNotificacoesService.GetUsuariosNotificacoes());
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
                await _processamentoNotificacoesService.ProcessarNotificacoes();
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex });
            }
        }
    }
}