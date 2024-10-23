using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using GreenRoofApi.Models;
using GreenRoofApi.Services;
using GreenRoofApi.DTOs;

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
        public async Task<IActionResult> CreatePedido([FromBody] PedidoDTO pedido)
        {
            var newPedido = await _pedidoService.CreateAsync(pedido);
            return CreatedAtAction(nameof(GetPedido), new { id = newPedido.Id }, newPedido);
        }

        // Atualizar status de pedido (Admin)
        [HttpPut("{id}/status")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateStatus(int id, [FromBody] string status)
        {
            var updatedPedido = await _pedidoService.UpdateStatusAsync(id,status);
            if (updatedPedido == null) return NotFound();
            return Ok(updatedPedido);
        }

        // Listar pedidos
        [HttpGet]
        public async Task<IActionResult> GetPedido(int id)
        {
            var pedido = await _pedidoService.GetByIdAsync(id);
            if (pedido == null) return NotFound();
            return Ok(pedido);
        }
    }
}

