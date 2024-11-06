using GreenRoofApi.DTOs;
using GreenRoofApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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

        [HttpGet]
        public async Task<IActionResult> GetProdutos()
        {
            var produtos = await _produtoService.GetAllAsync();
            return Ok(produtos);
        }

        // Adicionar produto (apenas Admin)
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddProduto([FromBody] ProdutosRequestDTO produto)
        {
            var newProduto = await _produtoService.CreateAsync(produto);
            return CreatedAtAction(nameof(GetProdutos), new { id = newProduto.Id }, newProduto);
        }

        // Atualizar produto (apenas Admin)
        [HttpPut("{id}/estoque")]
        [Authorize(Roles = "Admin, Funcionario")]
        public async Task<IActionResult> UpdateEstoque(int id, [FromBody] int quantidade)
        {
            var produto = await _produtoService.GetByIdAsync(id);
            if (produto == null) return NotFound();

            produto.Quantidade = quantidade;
            await _produtoService.UpdateAsync(produto);

            return NoContent();
        }
    }
}

