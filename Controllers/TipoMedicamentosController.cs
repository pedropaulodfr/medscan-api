using authentication_jwt.DTO;
using authentication_jwt.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace authentication_jwt.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TipoMedicamentosController : ControllerBase
    {
        private readonly TipoMedicamentoService _tipoMedicamentoService;

        public TipoMedicamentosController(TipoMedicamentoService tipoMedicamentoService)
        {
            _tipoMedicamentoService = tipoMedicamentoService;
        }

        [HttpGet]
        [Route("getAll")]
        public async Task<ActionResult> GetAll()
        {
            var result = await _tipoMedicamentoService.GetAll();  // Chama o método no serviço

            try
            {
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensagem = "Erro ao obter dados: " + ex });
            }
        }

        [Authorize(Policy = "AdminPolicy")]
        [HttpPost]
        [Route("insert")]
        public async Task<ActionResult> Insert([FromBody] TipoMedicamentosDTO model)
        {
            var result = await _tipoMedicamentoService.Insert(model);  // Chama o método no serviço

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
        [HttpPut]
        [Route("update")]
        public async Task<ActionResult> Update([FromBody] TipoMedicamentosDTO model)
        {
            var result = await _tipoMedicamentoService.Update(model);  // Chama o método no serviço

            try
            {
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensagem = "Erro ao atualizar: " + ex });
            }
        }
        
        [Authorize(Policy = "AdminPolicy")]
        [HttpDelete]
        [Route("delete/{id}")]
        public async Task<ActionResult> Delete(long id)
        {
            await _tipoMedicamentoService.Delete(id);  // Chama o método no serviço
            try
            {
                return Ok(new { mensagem = "Tipo de medicamento deletado com sucesso!" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensagem = "Erro ao deletar: " + ex });
            }
        }
    }
}
