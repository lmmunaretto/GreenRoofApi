using GreenRoofApi.Data;
using GreenRoofApi.DTOs;
using GreenRoofApi.Models;
using Microsoft.EntityFrameworkCore;

namespace GreenRoofApi.Services
{
    public class ProducaoCultivoService
    {
        private readonly GreenRoofContext _context;

        public ProducaoCultivoService(GreenRoofContext context)
        {
            _context = context;
        }

        // Obter todas as produções cadastradas
        public async Task<List<ProducaoCultivoDTO>> GetAllAsync()
        {
            var producoes = await _context.ProducaoCultivo
                .Include(p => p.Produto)
                .ToListAsync();

            return producoes.Select(p => new ProducaoCultivoDTO
            {
                Id = p.Id,
                ProdutoId = p.ProdutoId,
                ProdutoNome = p.Produto.Nome,
                AdminId = p.AdminId,
                DataProducao = p.DataProducao,
                QuantidadeProduzida = p.QuantidadeProduzida
            }).ToList();
        }

        // Obter uma produção específica por ID
        public async Task<ProducaoCultivoDTO> GetByIdAsync(int id)
        {
            var producao = await _context.ProducaoCultivo
                .Include(p => p.Produto)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (producao == null) return null;

            return new ProducaoCultivoDTO
            {
                Id = producao.Id,
                ProdutoId = producao.ProdutoId,
                ProdutoNome = producao.Produto.Nome,
                AdminId = producao.AdminId,
                DataProducao = producao.DataProducao,
                QuantidadeProduzida = producao.QuantidadeProduzida
            };
        }

        // Criar uma nova produção
        public async Task<ProducaoCultivo> CreateAsync(ProducaoCultivoRequestDTO producaoRequest, int adminId)
        {
            var produto = await _context.Produtos.FindAsync(producaoRequest.ProdutoId);

            if (produto == null)
            {
                throw new InvalidOperationException("Produto não encontrado.");
            }

            var producao = new ProducaoCultivo
            {
                ProdutoId = producaoRequest.ProdutoId,
                AdminId = adminId,
                DataProducao = producaoRequest.DataProducao,
                QuantidadeProduzida = producaoRequest.QuantidadeProduzida
            };

            // Adiciona a produção no banco de dados
            _context.ProducaoCultivo.Add(producao);

            // Atualiza o estoque do produto
            produto.Quantidade += producaoRequest.QuantidadeProduzida;

            _context.Produtos.Update(produto);

            await _context.SaveChangesAsync();

            return producao;
        }

        // Atualizar uma produção existente
        public async Task<ProducaoCultivo> UpdateAsync(int id, ProducaoCultivoDTO producaoRequest, int adminId)
        {
            var producao = await _context.ProducaoCultivo.FindAsync(id);
            if (producao == null) return null;

            producao.ProdutoId = producaoRequest.ProdutoId;
            producao.AdminId = adminId;
            producao.DataProducao = producaoRequest.DataProducao;
            producao.QuantidadeProduzida = producaoRequest.QuantidadeProduzida;

            _context.ProducaoCultivo.Update(producao);
            await _context.SaveChangesAsync();

            return producao;
        }

        // Deletar uma produção
        public async Task<bool> DeleteAsync(int id)
        {
            var producao = await _context.ProducaoCultivo.FindAsync(id);
            if (producao == null) return false;

            _context.ProducaoCultivo.Remove(producao);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
