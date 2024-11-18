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
            return await _context.Pedidos
                .Include(p => p.Cliente)
                .Include(p => p.ItemPedido)
                    .ThenInclude(i => i.Produto)
                .Select(p => new PedidoDTO
                {
                    Id = p.Id,
                    ClienteId = p.Cliente.Id,
                    ClienteNome = p.Cliente.Nome,
                    DataPedido = p.DataPedido,
                    TotalPedido = p.TotalPedido,
                    Status = p.Status,
                    ItemPedido = p.ItemPedido.Select(i => new ItemPedidoDTO
                    {
                        ProdutoId = i.Produto.Id,
                        ProdutoNome = i.Produto.Nome,
                        Quantidade = i.Quantidade,
                        PrecoUnitario = i.PrecoUnitario
                    }).ToList()
                })
                .ToListAsync();
        }

        public async Task<PedidoDTO?> GetByIdAsync(int id)
        {
            return await _context.Pedidos
                .Where(p => p.Id == id && p.Status != "Pago" && p.Status != "Concluído" && p.Status != "Cancelado")
                .Include(p => p.Cliente)
                .Include(p => p.ItemPedido)
                    .ThenInclude(i => i.Produto)
                .Select(p => new PedidoDTO
                {
                    Id = p.Id,
                    ClienteId = p.Cliente.Id,
                    ClienteNome = p.Cliente.Nome,
                    DataPedido = p.DataPedido,
                    TotalPedido = p.TotalPedido,
                    Status = p.Status,
                    ItemPedido = p.ItemPedido.Select(i => new ItemPedidoDTO
                    {
                        ProdutoId = i.Produto.Id,
                        ProdutoNome = i.Produto.Nome,
                        Quantidade = i.Quantidade,
                        PrecoUnitario = i.PrecoUnitario
                    }).ToList()
                })
                .FirstOrDefaultAsync();
        }



        public async Task<PedidoDTO?> GetAllByIdAsync(int id)
        {

            return await _context.Pedidos
                .Where(p => p.Id == id)
                .Include(p => p.Cliente)
                .Include(p => p.ItemPedido)
                    .ThenInclude(i => i.Produto)
                .Select(p => new PedidoDTO
                {
                    Id = p.Id,
                    ClienteId = p.Cliente.Id,
                    ClienteNome = p.Cliente.Nome,
                    DataPedido = p.DataPedido,
                    TotalPedido = p.TotalPedido,
                    Status = p.Status,
                    ItemPedido = p.ItemPedido.Select(i => new ItemPedidoDTO
                    {
                        ProdutoId = i.Produto.Id,
                        ProdutoNome = i.Produto.Nome,
                        Quantidade = i.Quantidade,
                        PrecoUnitario = i.PrecoUnitario
                    }).ToList()
                })
                .FirstOrDefaultAsync();
        }

        public async Task<Pedido> CreateAsync(PedidosRequestDTO pedidoDTO)
        {
            var pedido = new Pedido
            {
                ClienteId = pedidoDTO.ClienteId,
                DataPedido = DateTime.Now,
                TotalPedido = 0,
                Status = "Aguardando Processamento",
                ItemPedido = new List<ItemPedido>()
            };

            foreach (var itemDto in pedidoDTO.ItensPedido)
            {
                var produto = await _context.Produtos.FindAsync(itemDto.ProdutoId);

                if (produto == null || produto.Quantidade < itemDto.Quantidade)
                {
                    throw new Exception($"Estoque insuficiente para o produto: {produto?.Nome}");
                }
                produto.Quantidade -= itemDto.Quantidade;

                var itemPedido = new ItemPedido
                {
                    ProdutoId = itemDto.ProdutoId,
                    Quantidade = itemDto.Quantidade,
                    PrecoUnitario = itemDto.PrecoUnitario
                };
                pedido.ItemPedido.Add(itemPedido);
                pedido.TotalPedido += itemDto.PrecoUnitario * itemDto.Quantidade;
            }

            _context.Pedidos.Add(pedido);
            await _context.SaveChangesAsync();

            return pedido;
        }

        public async Task<Pedido?> UpdateAsync(int id, PedidosRequestDTO pedidoDTO)
        {
            var pedido = await _context.Pedidos.FindAsync(id);
            if (pedido == null)
            {
                return null;
            }

            pedido.ClienteId = pedidoDTO.ClienteId;
            pedido.DataPedido = pedidoDTO.DataPedido;
            pedido.TotalPedido = pedidoDTO.TotalPedido;
            pedido.Status = pedidoDTO.Status;

            _context.Pedidos.Update(pedido);
            await _context.SaveChangesAsync();

            return pedido;
        }

        public async Task<bool> UpdateStatusAsync(int pedidoId, string novoStatus)
        {
            var pedido = await _context.Pedidos.FindAsync(pedidoId);

            if (pedido == null)
                return false;

            pedido.Status = novoStatus;
            await _context.SaveChangesAsync();

            return true;
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
