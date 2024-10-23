using GreenRoofApi.Data;
using GreenRoofApi.DTOs;
using GreenRoofApi.Models;
using Microsoft.EntityFrameworkCore;

namespace GreenRoofApi.Controllers
{
    public class PagamentoService
    {
        private readonly GreenRoofContext _context;

        public PagamentoService(GreenRoofContext context)
        {
            _context = context;
        }

        public async Task<List<PagamentoDTO>> GetAllAsync()
        {
            var pagamentos = await _context.Pagamentos.ToListAsync();
            return pagamentos.Select(p => new PagamentoDTO
            {
                Id = p.Id,
                PedidoId = p.PedidoId,
                MetodoPagamento = p.MetodoPagamento,
                ValorPagamento = p.ValorPagamento,
                DataPagamento = p.DataPagamento,
                StatusPagamento = p.StatusPagamento
            }).ToList();
        }

        public async Task<PagamentoDTO> GetByIdAsync(int id)
        {
            var pagamento = await _context.Pagamentos.FindAsync(id);
            if (pagamento == null) return null;

            return new PagamentoDTO
            {
                Id = pagamento.Id,
                PedidoId = pagamento.PedidoId,
                MetodoPagamento = pagamento.MetodoPagamento,
                ValorPagamento = pagamento.ValorPagamento,
                DataPagamento = pagamento.DataPagamento,
                StatusPagamento = pagamento.StatusPagamento
            };
        }

        public async Task CreateAsync(PagamentoDTO pagamentoDTO)
        {
            var pagamento = new Pagamento
            {
                PedidoId = pagamentoDTO.PedidoId,
                MetodoPagamento = pagamentoDTO.MetodoPagamento,
                ValorPagamento = pagamentoDTO.ValorPagamento,
                DataPagamento = pagamentoDTO.DataPagamento,
                StatusPagamento = pagamentoDTO.StatusPagamento
            };

            _context.Pagamentos.Add(pagamento);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(int id, PagamentoDTO pagamentoDTO)
        {
            var pagamento = await _context.Pagamentos.FindAsync(id);
            if (pagamento == null) return;

            pagamento.PedidoId = pagamentoDTO.PedidoId;
            pagamento.MetodoPagamento = pagamentoDTO.MetodoPagamento;
            pagamento.ValorPagamento = pagamentoDTO.ValorPagamento;
            pagamento.DataPagamento = pagamentoDTO.DataPagamento;
            pagamento.StatusPagamento = pagamentoDTO.StatusPagamento;

            _context.Pagamentos.Update(pagamento);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var pagamento = await _context.Pagamentos.FindAsync(id);
            if (pagamento == null) return;

            _context.Pagamentos.Remove(pagamento);
            await _context.SaveChangesAsync();
        }
    }
}
