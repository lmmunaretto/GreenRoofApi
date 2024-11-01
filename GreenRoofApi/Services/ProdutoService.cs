using GreenRoofApi.Data;
using GreenRoofApi.DTOs;
using GreenRoofApi.Models;
using Microsoft.EntityFrameworkCore;

namespace GreenRoofApi.Services
{
    public class ProdutoService
    {
        private readonly GreenRoofContext _context;
        private readonly FornecedorService _fornecedorService;

        public ProdutoService(GreenRoofContext context, FornecedorService fornecedorService)
        {
            _context = context;
            _fornecedorService = fornecedorService;
        }

        public async Task<List<ProdutoDTO>> GetAllAsync()
        {
            var produtos = await _context.Produtos.ToListAsync();
            var produtosList = produtos.Select(p => new ProdutoDTO
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

        public async Task<ProdutoDTO> GetByIdAsync(int id)
        {
            var produto = await _context.Produtos.FindAsync(id);
            if (produto == null) return null;

            var fornecedor = await _fornecedorService.GetByIdAsync(produto.FornecedorId);

            return new ProdutoDTO
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

        public async Task<Produto> CreateAsync(ProdutosRequestDTO produtoDTO)
        {
            var produto = new Produto
            {
                Nome = produtoDTO.Nome,
                Descricao = produtoDTO.Descricao,
                Quantidade = produtoDTO.Quantidade,
                Preco = produtoDTO.Preco,
                Tipo = produtoDTO.Tipo,
                FornecedorId = produtoDTO.FornecedorId
            };

            _context.Produtos.Add(produto);
            await _context.SaveChangesAsync();

            return produto;
        }

        public async Task<Produto> UpdateAsync(int id, ProdutoDTO produtoDTO)
        {
            var produto = await _context.Produtos.FindAsync(id);
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

            _context.Produtos.Update(produto);
            await _context.SaveChangesAsync();

            return produto;
        }

        public async Task DeleteAsync(int id)
        {
            var produto = await _context.Produtos.FindAsync(id);
            if (produto == null) return;

            _context.Produtos.Remove(produto);
            await _context.SaveChangesAsync();
        }

    }
}

