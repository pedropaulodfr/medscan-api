using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using authentication_jwt.DTO;
using authentication_jwt.Services;
using Microsoft.AspNetCore.Mvc;

namespace authentication_jwt.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class EmailsController : ControllerBase
    {
        private readonly EmailService _emailService;

        public EmailsController(EmailService emailService)
        {
            _emailService = emailService;
        }

        [HttpGet]
        [Route("getAll")]
        public async Task<ActionResult> GetAll()
        {
            try
            {
                return StatusCode(200, await _emailService.GetAll());
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex });
            }
        }

        [HttpPut]
        [Route("update")]
        public async Task<ActionResult> Update([FromBody] EmailDTO model)
        {
            try
            {
                return StatusCode(200, await _emailService.Update(model));
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex });
            }
        }
    }
}