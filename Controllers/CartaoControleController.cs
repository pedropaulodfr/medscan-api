using authentication_jwt.DTO;
using authentication_jwt.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace authentication_jwt.Controllers
{
    [Route("[controller]")]
    public class CartaoControleController : Controller
    {
        private readonly CartaoControleService _cartaoControleService;

        public CartaoControleController(CartaoControleService cartaoControleService)
        {
            _cartaoControleService = cartaoControleService;
        }

        [Authorize]
        [HttpGet]
        [Route("get/{PacienteId}")]
        public async Task<ActionResult> Get(long PacienteId)
        {
            try
            {
                return Ok(await _cartaoControleService.Get(PacienteId));
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensagem = "Erro ao salvar: " + ex });
            }
        }

        [Authorize(Policy = "AdminPolicy")]
        [HttpGet]
        [Route("getAll")]
        public async Task<ActionResult> GetAll()
        {
            try
            {
                return Ok(await _cartaoControleService.GetAll());
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensagem = "Erro ao salvar: " + ex });
            }
        }
        
        [Authorize]
        [HttpPost]
        [Route("insert")]
        public async Task<ActionResult> Insert([FromBody] CartaoControleDTO model)
        {
            try
            {
                return Ok(await _cartaoControleService.Insert(model));
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensagem = "Erro ao salvar: " + ex });
            }
        }

        [Authorize]
        [HttpPut]
        [Route("update")]
        public async Task<ActionResult> Update([FromBody] CartaoControleDTO model)
        {
            try
            {
                return Ok(await _cartaoControleService.Update(model));
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensagem = "Erro ao atualizar: " + ex });
            }
        }
        
        [Authorize]
        [HttpDelete]
        [Route("delete/{id}")]
        public async Task<ActionResult> Delete(long id)
        {
            try
            {
                await _cartaoControleService.Delete(id);  // Chama o método no serviço
                return Ok(new { mensagem = "Registro deletado com sucesso!" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensagem = "Erro ao salvar: " + ex });
            }
        }
    }
}