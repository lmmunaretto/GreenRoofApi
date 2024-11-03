using GreenRoofApi.Data;
using GreenRoofApi.DTOs;
using GreenRoofApi.Models;
using Microsoft.EntityFrameworkCore;

namespace GreenRoofApi.Services
{
    public class ItensPedidoService
    {
        private readonly GreenRoofContext _context;

        public ItensPedidoService(GreenRoofContext context)
        {
            _context = context;
        }

        public async Task<List<ItemPedidoDTO>> GetAllAsync()
        {
            var itemPedidos = await _context.ItensPedidos.ToListAsync();
            return itemPedidos.Select(ip => new ItemPedidoDTO
            {
                Id = ip.Id,
                PedidoId = ip.PedidoId,
                ProdutoId = ip.ProdutoId,
                Quantidade = ip.Quantidade,
                PrecoUnitario = ip.PrecoUnitario
            }).ToList();
        }

        public async Task<ItemPedidoDTO> GetByIdAsync(int id)
        {
            var itemPedido = await _context.ItensPedidos.FindAsync(id);
            if (itemPedido == null) return null;

            return new ItemPedidoDTO
            {
                Id = itemPedido.Id,
                PedidoId = itemPedido.PedidoId,
                ProdutoId = itemPedido.ProdutoId,
                Quantidade = itemPedido.Quantidade,
                PrecoUnitario = itemPedido.PrecoUnitario
            };
        }

        public async Task<List<ItemPedidoDTO>> GetItemPedidoByProdutoId(int pedidoId, int produtoId)
        {
            var itemPedidos = await _context.ItensPedidos
                                            .Where(itemPedido => itemPedido.PedidoId == pedidoId && itemPedido.ProdutoId == produtoId)
                                            .ToListAsync();

            var result = itemPedidos.Select(itemPedido => new ItemPedidoDTO
            {
                Id = itemPedido.Id,
                PedidoId = itemPedido.PedidoId,
                ProdutoId = itemPedido.ProdutoId,
                Quantidade = itemPedido.Quantidade,
                PrecoUnitario = itemPedido.PrecoUnitario
            }).ToList();

            return result;
        }

        public async Task CreateAsync(ItensPedidosDTO itemPedidoDTO)
        {
            var itemPedido = new ItemPedido
            {
                PedidoId = itemPedidoDTO.PedidoId,
                ProdutoId = itemPedidoDTO.ProdutoId,
                Quantidade = itemPedidoDTO.Quantidade,
                PrecoUnitario = itemPedidoDTO.PrecoUnitario
            };

            _context.ItensPedidos.Add(itemPedido);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(int id, ItemPedidoDTO itemPedidoDTO)
        {
            var itemPedido = await _context.ItensPedidos.FindAsync(id);
            if (itemPedido == null) return;

            itemPedido.Quantidade = itemPedidoDTO.Quantidade;
            itemPedido.PrecoUnitario = itemPedidoDTO.PrecoUnitario;

            _context.ItensPedidos.Update(itemPedido);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var itemPedido = await _context.ItensPedidos.FindAsync(id);
            if (itemPedido == null) return;

            _context.ItensPedidos.Remove(itemPedido);
            await _context.SaveChangesAsync();
        }
    }
}
