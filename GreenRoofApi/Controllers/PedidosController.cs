using GreenRoofApi.DTOs;
using GreenRoofApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GreenRoofApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PedidosController : ControllerBase
    {
        private readonly PedidoService _pedidoService;

        public PedidosController(PedidoService pedidoService)
        {
            _pedidoService = pedidoService;
        }

        // Atualizar status de pedido (Admin)
        [HttpPut("{id}/status")]
        public async Task<IActionResult> UpdateStatus(int id, [FromBody] string novoStatus)
        {
            var resultado = await _pedidoService.UpdateStatusAsync(id, novoStatus);

            if (!resultado)
                return NotFound("Pedido não encontrado.");

            return NoContent();
        }

        // Atualizar o pedido
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePedido(int id, [FromBody] PedidosRequestDTO pedido)
        {
            var updatedPedido = await _pedidoService.UpdateAsync(id, pedido);
            if (updatedPedido == null) return NotFound();
            return Ok(updatedPedido);
        }

        // Listar pedidos
        [HttpGet("{id}")]
        public async Task<IActionResult> GetPedido(int id)
        {
            var pedido = await _pedidoService.GetByIdAsync(id);
            if (pedido == null) return NotFound();
            return Ok(pedido);
        }

        // Listar pedidos
        [HttpGet("all/{id}")]
        public async Task<IActionResult> GetAllPedido(int id)
        {
            var pedido = await _pedidoService.GetAllByIdAsync(id);
            if (pedido == null) return NotFound();
            return Ok(pedido);
        }

        [HttpGet]
        public async Task<IActionResult> GetPedidos()
        {
            var pedidos = await _pedidoService.GetAllAsync();
            return Ok(pedidos.Select(p => new
            {
                p.Id,
                p.ClienteNome,
                p.ClienteId,
                p.DataPedido,
                p.TotalPedido,
                p.Status,
                Itens = p.ItemPedido.Select(i => new
                {
                    i.ProdutoNome,
                    i.Quantidade,
                    i.PrecoUnitario
                }).ToList()
            }));
        }


        [HttpPost]
        [Authorize(Roles = "Admin, Cliente")]
        public async Task<IActionResult> CreatePedido([FromBody] PedidosRequestDTO pedido)
        {
            var newPedido = await _pedidoService.CreateAsync(pedido);
            return CreatedAtAction(nameof(GetPedido), new { id = newPedido.Id }, newPedido);
        }
    }
}

