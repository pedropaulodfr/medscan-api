using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using authentication_jwt.DTO;
using authentication_jwt.Models;
using authentication_jwt.Services;
using Microsoft.AspNetCore.Mvc;

namespace authentication_jwt.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TratamentosController : ControllerBase
    {
        private readonly TratamentosService _tratamentosService;

        public TratamentosController(TratamentosService tratamentosService)
        {
            _tratamentosService = tratamentosService;
        }

        [HttpGet]
        [Route("{PacienteId?}")]
        public async Task<ActionResult> Get(long? PacienteId)
        {
            try
            {
                return StatusCode(200, await _tratamentosService.Get(PacienteId));
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
        [HttpPost]
        public async Task<ActionResult> Insert(TratamentosDTO model)
        {
            try
            {
                return StatusCode(200, await _tratamentosService.Insert(model));
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut]
        [Route("update")]
        public async Task<ActionResult> Update(TratamentosDTO model)
        {
            try
            {
                return StatusCode(200, await _tratamentosService.Update(model));
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpDelete]
        [Route("delete/{id}")]
        public async Task<ActionResult> Delete(long id)
        {
            await _tratamentosService.Delete(id);  // Chama o método no serviço
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