using GreenRoofApi.DTOs;
using GreenRoofApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace GreenRoofApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProdutosController : ControllerBase
    {
        private readonly ProdutoService _produtoService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ProdutosController(ProdutoService produtoService, IHttpContextAccessor httpContextAccessor)
        {
            _produtoService = produtoService;
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpGet]
        public async Task<IActionResult> GetProdutos()
        {
            var produtos = await _produtoService.GetAllAsync();
            return Ok(produtos);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProdutoById(int id)
        {
            var produtos = await _produtoService.GetByIdAsync(id);
            return Ok(produtos);
        }

        // Adicionar produto (apenas Admin)
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddProduto([FromBody] ProdutosRequestDTO produto)
        {
            var userId = _httpContextAccessor.HttpContext.User.FindFirstValue("usuarioId");

            var newProduto = await _produtoService.CreateAsync(produto, int.Parse(userId));
            return CreatedAtAction(nameof(GetProdutos), new { id = newProduto.Id }, newProduto);
        }

        // Atualizar produto
        [HttpPut("{id}/estoque")]
        [Authorize(Roles = "Admin, Cliente")]
        public async Task<IActionResult> UpdateEstoque(int id, [FromBody] int quantidade)
        {
            var userId = _httpContextAccessor.HttpContext.User.FindFirstValue("usuarioId");
            var produto = await _produtoService.GetByIdAsync(id);
            if (produto == null) return NotFound();

            produto.Quantidade = quantidade;
            await _produtoService.UpdateAsync(produto, int.Parse(userId));

            return NoContent();
        }
    }
}

