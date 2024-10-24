﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using GreenRoofApi.Models;
using GreenRoofApi.Services;
using GreenRoofApi.DTOs;

namespace GreenRoofApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProdutosController : ControllerBase
    {
        private readonly ProdutoService _produtoService;

        public ProdutosController(ProdutoService produtoService)
        {
            _produtoService = produtoService;
        }

        // Listar produtos (aberto)
        [HttpGet]
        public async Task<IActionResult> GetProdutos()
        {
            var produtos = await _produtoService.GetAllAsync();
            return Ok(produtos);
        }

        // Adicionar produto (apenas Admin)
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddProduto([FromBody] ProdutoDTO produto)
        {
            var newProduto = await _produtoService.CreateAsync(produto);
            return CreatedAtAction(nameof(GetProdutos), new { id = newProduto.Id }, newProduto);
        }

        // Atualizar produto (apenas Admin)
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateProduto(int id, [FromBody] ProdutoDTO produto)
        {
            if (id != produto.Id) return BadRequest();

            var updatedProduto = await _produtoService.UpdateAsync(id, produto);
            return Ok(updatedProduto);
        }

        // Deletar produto (apenas Admin)
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteProduto(int id)
        {
            await _produtoService.DeleteAsync(id);
            return NoContent();
        }
    }
}

