using GreenRoofApi.Data;
using GreenRoofApi.DTOs;
using GreenRoofApi.Models;
using Microsoft.EntityFrameworkCore;

namespace GreenRoofApi.Services
{
    public class PedidoService
    {
        private readonly GreenRoofContext _context;
        private readonly ItensPedidoService _itensPedidoService;
        private readonly ProdutoService _produtoService;

        public PedidoService(GreenRoofContext context, ItensPedidoService itensPedidoService, ProdutoService produtoService)
        {
            _context = context;
            _itensPedidoService = itensPedidoService;
            _produtoService = produtoService;
        }

        public async Task<List<PedidoDTO>> GetAllAsync()
        {
            var pedidos = await _context.Pedidos.ToListAsync();
            var pedidosList = pedidos.Select(p => new PedidoDTO
            {
                Id = p.Id,
                ClienteId = p.ClienteId,
                DataPedido = p.DataPedido,
                Total = p.Total,
                Status = p.Status
            }).ToList();

            foreach (var p in pedidosList)
            {
                var itens = await _itensPedidoService.GetAllAsync();
                var itensPedido = itens.FindAll(x => x.PedidoId == p.Id);

                foreach (var item in itensPedido)
                {
                    var produtos = await _produtoService.GetByIdAsync(item.ProdutoId);
                    item.Produto = produtos;
                }

                p.ItensPedido = itensPedido;
            }

            return pedidosList;
        }

        public async Task<PedidoDTO> GetByIdAsync(int id)
        {
            var pedido = await _context.Pedidos.FindAsync(id);
            if (pedido == null) return null;

            var itens = await _itensPedidoService.GetAllAsync();
            var itensPedido = itens.FindAll(x => x.PedidoId == pedido.Id).ToList();

            foreach (var item in itensPedido)
            {
                var produtos = await _produtoService.GetByIdAsync(item.ProdutoId);
                item.Produto = produtos;
            }

            return new PedidoDTO
            {
                Id = pedido.Id,
                ClienteId = pedido.ClienteId,
                DataPedido = pedido.DataPedido,
                Total = pedido.Total,
                Status = pedido.Status,
                ItensPedido = itensPedido
            };
        }

        public async Task<Pedido> CreateAsync(PedidosRequestDTO pedidoDTO)
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

        public async Task<Pedido> UpdateAsync(int id, PedidosRequestDTO pedidoDTO)
        {
            var pedido = await _context.Pedidos.FindAsync(id);
            if (pedido == null)
            {
                return null;
            }

            pedido.ClienteId = pedidoDTO.ClienteId;
            pedido.DataPedido = pedidoDTO.DataPedido;
            pedido.Total = pedidoDTO.Total;
            pedido.Status = pedidoDTO.Status;

            _context.Pedidos.Update(pedido);
            await _context.SaveChangesAsync();

            return pedido;
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
