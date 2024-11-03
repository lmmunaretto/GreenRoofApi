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

        // Criar pedido (Cliente)
        [HttpPost]
        [Authorize(Roles = "Cliente")]
        public async Task<IActionResult> CreatePedido([FromBody] PedidosRequestDTO pedido)
        {
            var newPedido = await _pedidoService.CreateAsync(pedido);
            return CreatedAtAction(nameof(GetPedido), new { id = newPedido.Id }, newPedido);
        }

        // Atualizar status de pedido (Admin)
        [HttpPut("{id}/status")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateStatus(int id, [FromBody] string status)
        {
            var updatedPedido = await _pedidoService.UpdateStatusAsync(id, status);
            if (updatedPedido == null) return NotFound();
            return Ok(updatedPedido);
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

        [HttpGet]
        public async Task<IActionResult> GetPedidos()
        {
            var pedido = await _pedidoService.GetAllAsync();
            if (pedido == null) return NotFound();
            return Ok(pedido);
        }
    }
}

