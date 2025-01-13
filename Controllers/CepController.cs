using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using authentication_jwt.Services;
using Microsoft.AspNetCore.Mvc;

namespace authentication_jwt.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CepController : ControllerBase
    {
        private readonly CepService _cepService;

        public CepController(CepService cepService)
        {
            _cepService = cepService;
        }

        [HttpGet]
        [Route("getEndereco/{CEP}")]
        public async Task<ActionResult> GetEndereco(string CEP)
        {
            try
            {
                return StatusCode(200,  await _cepService.GetEndereco(CEP));
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex });
            }
        }
    }
}