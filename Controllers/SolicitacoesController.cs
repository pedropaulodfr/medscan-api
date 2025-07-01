using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using authentication_jwt.DTO;
using authentication_jwt.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace authentication_jwt.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SolicitacoesController : ControllerBase
    {
        private readonly SolicitacoesService _solicitacoesService;
        public SolicitacoesController(SolicitacoesService solicitacoesService)
        {
            _solicitacoesService = solicitacoesService;
        }

        [Authorize]
        [HttpGet]
        [Route("getAll/{Status}/{PacienteId?}")]
        public async Task<ActionResult> GetAll(string Status, long? PacienteId)
        {
            try
            {
                return StatusCode(200, await _solicitacoesService.GetAll(Status, PacienteId));
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [Authorize]
        [HttpPost]
        [Route("insert")]
        public async Task<ActionResult> Insert(SolicitacoesDTO model)
        {
            try
            {
                return StatusCode(200, await _solicitacoesService.Insert(model));
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [Authorize(Policy = "AdminPolicy")]
        [HttpGet]
        [Route("analiseSolicitacao/{SolicitacaoId}/{Aprovado}")]
        public async Task<ActionResult> AnaliseSolicitacao(long SolicitacaoId, bool Aprovado)
        {
            try
            {
                await _solicitacoesService.AnaliseSolicitacao(SolicitacaoId, Aprovado);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [Authorize]
        [HttpDelete]
        [Route("delete/{SolicitacaoId}")]
        public async Task<ActionResult> Delete(long SolicitacaoId)
        {
            try
            {
                await _solicitacoesService.Delete(SolicitacaoId);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}