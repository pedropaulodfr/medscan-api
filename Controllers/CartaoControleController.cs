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

        [HttpGet]
        [Route("get/{PacienteId}")]
        public async Task<ActionResult> Get(long PacienteId)
        {
            var result = await _cartaoControleService.Get(PacienteId);  // Chama o método no serviço

            try
            {
                return Ok(result);
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
            var result = await _cartaoControleService.GetAll();  // Chama o método no serviço

            try
            {
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensagem = "Erro ao salvar: " + ex });
            }
        }
        
        [HttpPost]
        [Route("insert")]
        public async Task<ActionResult> Insert([FromBody] CartaoControleDTO model)
        {
            var result = await _cartaoControleService.Insert(model);  // Chama o método no serviço

            try
            {
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensagem = "Erro ao salvar: " + ex });
            }
        }

        [HttpPut]
        [Route("update")]
        public async Task<ActionResult> Update([FromBody] CartaoControleDTO model)
        {
            var result = await _cartaoControleService.Update(model);  // Chama o método no serviço

            try
            {
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensagem = "Erro ao atualizar: " + ex });
            }
        }
        
        [HttpDelete]
        [Route("delete/{id}")]
        public async Task<ActionResult> Delete(long id)
        {
            await _cartaoControleService.Delete(id);  // Chama o método no serviço

            try
            {
                return Ok(new { mensagem = "Registro deletado com sucesso!" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensagem = "Erro ao salvar: " + ex });
            }
        }
    }
}