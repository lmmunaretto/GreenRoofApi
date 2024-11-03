using GreenRoofApi.DTOs;
using GreenRoofApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace GreenRoofApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ItensPedidoController : ControllerBase
    {
        private readonly ItensPedidoService _itensPedidoService;

        public ItensPedidoController(ItensPedidoService itensPedidoService)
        {
            _itensPedidoService = itensPedidoService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ItemPedidoDTO>>> GetAll()
        {
            var itensPedido = await _itensPedidoService.GetAllAsync();
            return Ok(itensPedido);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ItemPedidoDTO>> GetById(int id)
        {
            var itemPedido = await _itensPedidoService.GetByIdAsync(id);
            if (itemPedido == null)
            {
                return NotFound();
            }
            return Ok(itemPedido);
        }

        [HttpGet("query")]
        public async Task<ActionResult<ItemPedidoDTO>> GetItemPedidoByProdutoId([FromQuery] int pedidoId, [FromQuery] int produtoId)
        {
            var itemPedido = await _itensPedidoService.GetItemPedidoByProdutoId(pedidoId, produtoId);
            if (itemPedido == null)
            {
                return NotFound();
            }
            return Ok(itemPedido);
        }

        [HttpPost]
        public async Task<ActionResult> Create([FromBody] ItensPedidosDTO itemPedidoDTO)
        {
            await _itensPedidoService.CreateAsync(itemPedidoDTO);
            return Ok();
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int id, [FromBody] ItemPedidoDTO itemPedidoDTO)
        {
            await _itensPedidoService.UpdateAsync(id, itemPedidoDTO);
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            await _itensPedidoService.DeleteAsync(id);
            return Ok();
        }
    }
}
