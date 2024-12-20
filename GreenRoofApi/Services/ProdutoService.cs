﻿using GreenRoofApi.Data;
using GreenRoofApi.DTOs;
using GreenRoofApi.Models;
using Microsoft.EntityFrameworkCore;

namespace GreenRoofApi.Services
{
    public class ProdutoService
    {
        private readonly GreenRoofContext _context;
        private readonly InformacaoNutricionalService _infoNutriService;

        public ProdutoService(GreenRoofContext context, InformacaoNutricionalService infoNutriService)
        {
            _context = context;
            _infoNutriService = infoNutriService;
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
                Tipo = p.Tipo
            }).ToList();

            foreach (var item in produtosList)
            {
                var infoNutri = await _infoNutriService.GetByProdutoIdAsync(item.Id);
                item.InformacoesNutricionais = infoNutri;
            }

            return produtosList;
        }

        public async Task<ProdutoDTO> GetByIdAsync(int id)
        {
            var produto = await _context.Produtos.FindAsync(id);
            if (produto == null) return null;

            var infoNutri = await _infoNutriService.GetByProdutoIdAsync(produto.Id);

            return new ProdutoDTO
            {
                Id = produto.Id,
                Nome = produto.Nome,
                Descricao = produto.Descricao,
                Quantidade = produto.Quantidade,
                Preco = produto.Preco,
                Tipo = produto.Tipo,
                InformacoesNutricionais = infoNutri,
            };
        }

        public async Task<Produto> CreateAsync(ProdutosRequestDTO produtoDTO, int usuarioId)
        {
            var produto = new Produto
            {
                Nome = produtoDTO.Nome,
                Descricao = produtoDTO.Descricao,
                Quantidade = produtoDTO.Quantidade,
                Preco = produtoDTO.Preco,
                Tipo = produtoDTO.Tipo,
                AdminId = usuarioId,
                LimiteMinimoEstoque = produtoDTO.LimiteMinimoEstoque,
            };

            _context.Produtos.Add(produto);
            await _context.SaveChangesAsync();

            if (produto.Quantidade <= produto.LimiteMinimoEstoque)
            {
                EnviarAlertaEstoqueBaixo(produto);
            }
            return produto;
        }
        public void EnviarAlertaEstoqueBaixo(Produto produto)
        {
            Console.WriteLine($"Alerta: Estoque baixo para o produto '{produto.Nome}'");
        }

        public async Task<Produto> UpdateAsync(ProdutoDTO produtoDTO, int usuarioId)
        {
            var produto = await _context.Produtos.FindAsync(produtoDTO.Id);
            if (produto == null)
            {
                return null;
            }

            produto.Nome = produtoDTO.Nome;
            produto.Descricao = produtoDTO.Descricao;
            produto.Quantidade = produtoDTO.Quantidade;
            produto.Preco = produtoDTO.Preco;
            produto.Tipo = produtoDTO.Tipo;
            produto.AdminId = usuarioId;

            _context.Produtos.Update(produto);
            await _context.SaveChangesAsync();

            if (produto.Quantidade <= produto.LimiteMinimoEstoque)
            {
                EnviarAlertaEstoqueBaixo(produto);
            }

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

