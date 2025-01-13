using authentication_jwt.DTO;
using authentication_jwt.Services;
using Microsoft.AspNetCore.Mvc;

namespace authentication_jwt.Controllers
{
    [Route("[controller]")]
    public class MedicamentosController : Controller
    {
        private readonly MedicamentosService _medicamentosService;

        public MedicamentosController(MedicamentosService medicamentosService)
        {
            _medicamentosService = medicamentosService;
        }

        [HttpGet]
        [Route("getAll")]
        public async Task<ActionResult> GetAll()
        {
            var result = await _medicamentosService.GetAll();  // Chama o método no serviço

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
        public async Task<ActionResult> Insert([FromBody] MedicamentoDTO model)
        {
            var result = await _medicamentosService.Insert(model);  // Chama o método no serviço

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
        public async Task<ActionResult> Update([FromBody] MedicamentoDTO model)
        {
            var result = await _medicamentosService.Update(model);  // Chama o método no serviço

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
            await _medicamentosService.Delete(id);  // Chama o método no serviço

            try
            {
                return Ok(new { mensagem = "Medicamento deletado com sucesso!" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensagem = "Erro ao salvar: " + ex });
            }
        }
    }
}