using GreenRoofApi.Data;
using GreenRoofApi.DTOs;
using GreenRoofApi.Models;
using Microsoft.EntityFrameworkCore;

namespace GreenRoofApi.Services
{
    public class PedidoService
    {
        private readonly GreenRoofContext _context;

        public PedidoService(GreenRoofContext context)
        {
            _context = context;
        }

        public async Task<List<PedidoDTO>> GetAllAsync()
        {
            var pedidos = await _context.Pedidos.ToListAsync();
            return pedidos.Select(p => new PedidoDTO
            {
                Id = p.Id,
                ClienteId = p.ClienteId,
                DataPedido = p.DataPedido,
                Total = p.Total,
                Status = p.Status
            }).ToList();
        }

        public async Task<PedidoDTO> GetByIdAsync(int id)
        {
            var pedido = await _context.Pedidos.FindAsync(id);
            if (pedido == null) return null;

            return new PedidoDTO
            {
                Id = pedido.Id,
                ClienteId = pedido.ClienteId,
                DataPedido = pedido.DataPedido,
                Total = pedido.Total,
                Status = pedido.Status
            };
        }

        public async Task<Pedido> CreateAsync(PedidoDTO pedidoDTO)
        {
            var pedido = new Pedido
            {
                ClienteId = pedidoDTO.ClienteId,
                DataPedido = pedidoDTO.DataPedido,
                Total = pedidoDTO.Total,
                Status = pedidoDTO.Status
            };

            _context.Pedidos.Add(pedido);
            await _context.SaveChangesAsync();

            return pedido;
        }

        public async Task UpdateAsync(int id, PedidoDTO pedidoDTO)
        {
            var pedido = await _context.Pedidos.FindAsync(id);
            if (pedido == null) return;

            pedido.ClienteId = pedidoDTO.ClienteId;
            pedido.DataPedido = pedidoDTO.DataPedido;
            pedido.Total = pedidoDTO.Total;
            pedido.Status = pedidoDTO.Status;

            _context.Pedidos.Update(pedido);
            await _context.SaveChangesAsync();
        }

        public async Task<Pedido> UpdateStatusAsync(int id, string status)
        {
            var pedido = await _context.Pedidos.FindAsync(id);
            if (pedido == null)
            {
                return null;
            }

            pedido.Status = status;

            _context.Pedidos.Update(pedido);
            await _context.SaveChangesAsync();

            return pedido;
        }

        public async Task DeleteAsync(int id)
        {
            var pedido = await _context.Pedidos.FindAsync(id);
            if (pedido == null) return;

            _context.Pedidos.Remove(pedido);
            await _context.SaveChangesAsync();
        }
    }
}
