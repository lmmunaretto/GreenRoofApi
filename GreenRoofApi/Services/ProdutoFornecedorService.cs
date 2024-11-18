using GreenRoofApi.Data;
using GreenRoofApi.DTOs;
using GreenRoofApi.Models;
using Microsoft.EntityFrameworkCore;

namespace GreenRoofApi.Services
{
    public class ProdutoFornecedorService
    {
        private readonly GreenRoofContext _context;
        private readonly FornecedorService _fornecedorService;

        public ProdutoFornecedorService(GreenRoofContext context, FornecedorService fornecedorService)
        {
            _context = context;
            _fornecedorService = fornecedorService;
        }

        public async Task<List<ProdutoFornecedorDTO>> GetAllAsync()
        {
            var produtos = await _context.ProdutosFornecedor.ToListAsync();
            var produtosList = produtos.Select(p => new ProdutoFornecedorDTO
            {
                Id = p.Id,
                Nome = p.Nome,
                Descricao = p.Descricao,
                Quantidade = p.Quantidade,
                Preco = p.Preco,
                Tipo = p.Tipo,
                FornecedorId = p.FornecedorId
            }).ToList();

            foreach (var item in produtosList)
            {
                var fornecedor = await _fornecedorService.GetByIdAsync(item.FornecedorId);
                item.Fornecedor = fornecedor;
            }

            return produtosList;
        }

        public async Task<ProdutoFornecedorDTO> GetByIdAsync(int id)
        {
            var produto = await _context.ProdutosFornecedor.FindAsync(id);
            if (produto == null) return null;

            var fornecedor = await _fornecedorService.GetByIdAsync(produto.FornecedorId);

            return new ProdutoFornecedorDTO
            {
                Id = produto.Id,
                Nome = produto.Nome,
                Descricao = produto.Descricao,
                Quantidade = produto.Quantidade,
                Preco = produto.Preco,
                Tipo = produto.Tipo,
                FornecedorId = produto.FornecedorId,
                Fornecedor = fornecedor,
            };
        }

        public async Task<ProdutoFornecedor> CreateAsync(ProdutosFornecedorRequestDTO produtoDTO)
        {
            var produto = new ProdutoFornecedor
            {
                Nome = produtoDTO.Nome,
                Descricao = produtoDTO.Descricao,
                Quantidade = produtoDTO.Quantidade,
                Preco = produtoDTO.Preco,
                Tipo = produtoDTO.Tipo,
                FornecedorId = produtoDTO.FornecedorId,
            };

            _context.ProdutosFornecedor.Add(produto);
            await _context.SaveChangesAsync();

            return produto;
        }
        public void EnviarAlertaEstoqueBaixo(Produto produto)
        {
            Console.WriteLine($"Alerta: Estoque baixo para o produto '{produto.Nome}'");
        }

        public async Task<ProdutoFornecedor> UpdateAsync(ProdutoFornecedorDTO produtoDTO)
        {
            var produto = await _context.ProdutosFornecedor.FindAsync(produtoDTO.Id);
            if (produto == null)
            {
                return null;
            }

            produto.Nome = produtoDTO.Nome;
            produto.Descricao = produtoDTO.Descricao;
            produto.Quantidade = produtoDTO.Quantidade;
            produto.Preco = produtoDTO.Preco;
            produto.Tipo = produtoDTO.Tipo;
            produto.FornecedorId = produtoDTO.FornecedorId;

            _context.ProdutosFornecedor.Update(produto);
            await _context.SaveChangesAsync();

            return produto;
        }

        public async Task DeleteAsync(int id)
        {
            var produto = await _context.ProdutosFornecedor.FindAsync(id);
            if (produto == null) return;

            _context.ProdutosFornecedor.Remove(produto);
            await _context.SaveChangesAsync();
        }

    }
}

