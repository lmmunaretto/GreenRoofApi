using GreenRoofApi.Data;
using GreenRoofApi.DTOs;
using GreenRoofApi.Models;

namespace GreenRoofApi.Services
{
    public class ProdutoService
    {
        private readonly GreenRoofContext _context;

        public ProdutoService(GreenRoofContext context)
        {
            _context = context;
        }

        public async Task<List<ProdutoDTO>> GetAllAsync()
        {
            var produtos = await _context.Produtos.ToListAsync();
            return produtos.Select(p => new ProdutoDTO
            {
                Id = p.Id,
                Nome = p.Nome,
                Descricao = p.Descricao,
                Quantidade = p.Quantidade,
                Preco = p.Preco,
                Tipo = p.Tipo,
                FornecedorId = p.FornecedorId
            }).ToList();
        }

        public async Task<ProdutoDTO> GetByIdAsync(int id)
        {
            var produto = await _context.Produtos.FindAsync(id);
            if (produto == null) return null;

            return new ProdutoDTO
            {
                Id = produto.Id,
                Nome = produto.Nome,
                Descricao = produto.Descricao,
                Quantidade = produto.Quantidade,
                Preco = produto.Preco,
                Tipo = produto.Tipo,
                FornecedorId = produto.FornecedorId
            };
        }

        public async Task CreateAsync(ProdutoDTO produtoDTO)
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
        }

        public async Task UpdateAsync(int id, ProdutoDTO produtoDTO)
        {
            var produto = await _context.Produtos.FindAsync(id);
            if (produto == null) return;

            produto.Nome = produtoDTO.Nome;
            produto.Descricao = produtoDTO.Descricao;
            produto.Quantidade = produtoDTO.Quantidade;
            produto.Preco = produtoDTO.Preco;
            produto.Tipo = produtoDTO.Tipo;
            produto.FornecedorId = produtoDTO.FornecedorId;

            _context.Produtos.Update(produto);
            await _context.SaveChangesAsync();
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

