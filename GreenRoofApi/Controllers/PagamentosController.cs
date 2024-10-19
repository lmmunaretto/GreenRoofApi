using GreenRoofApi.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace GreenRoofApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PagamentosController : ControllerBase
    {
        private readonly PagamentoService _pagamentoService;

        public PagamentosController(PagamentoService pagamentoService)
        {
            _pagamentoService = pagamentoService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PagamentoDTO>>> GetAll()
        {
            var pagamentos = await _pagamentoService.GetAllAsync();
            return Ok(pagamentos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PagamentoDTO>> GetById(int id)
        {
            var pagamento = await _pagamentoService.GetByIdAsync(id);
            if (pagamento == null)
            {
                return NotFound();
            }
            return Ok(pagamento);
        }

        [HttpPost]
        public async Task<ActionResult> Create([FromBody] PagamentoDTO pagamentoDTO)
        {
            await _pagamentoService.CreateAsync(pagamentoDTO);
            return Ok();
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int id, [FromBody] PagamentoDTO pagamentoDTO)
        {
            await _pagamentoService.UpdateAsync(id, pagamentoDTO);
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            await _pagamentoService.DeleteAsync(id);
            return Ok();
        }
    }
}
