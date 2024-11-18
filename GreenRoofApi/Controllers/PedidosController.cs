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

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPedido(int id)
        {
            var pedido = await _pedidoService.GetByIdAsync(id);
            if (pedido == null) return NotFound();

            return Ok(new
            {
                pedido.Id,
                pedido.ClienteId,
                pedido.ClienteNome,
                pedido.DataPedido,
                pedido.TotalPedido,
                pedido.Status,
                itemPedido = pedido.ItemPedido.Select(i => new
                {
                    i.Id,
                    i.PedidoId,
                    i.ProdutoId,
                    i.ProdutoNome,
                    i.Quantidade,
                    i.PrecoUnitario
                }).ToList()
            });
        }

        // Listar todos os pedidos de um cliente por ID
        [HttpGet("all/{id}")]
        public async Task<IActionResult> GetAllPedido(int id)
        {
            var pedidos = await _pedidoService.GetAllByIdAsync(id);
            if (pedidos == null) return NotFound();

            return Ok(new
            {
                pedidos.Id,
                pedidos.ClienteId,
                pedidos.ClienteNome,
                pedidos.DataPedido,
                pedidos.TotalPedido,
                pedidos.Status,
                itemPedido = pedidos.ItemPedido.Select(i => new
                {
                    i.Id,
                    i.PedidoId,
                    i.ProdutoId,
                    i.ProdutoNome,
                    i.Quantidade,
                    i.PrecoUnitario
                }).ToList()
            });
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
                itemPedido = p.ItemPedido.Select(i => new
                {
                    i.Id,
                    i.PedidoId,
                    i.ProdutoId,
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

