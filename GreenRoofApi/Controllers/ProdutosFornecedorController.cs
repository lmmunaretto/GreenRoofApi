using GreenRoofApi.DTOs;
using GreenRoofApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace GreenRoofApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProdutosFornecedorController : ControllerBase
    {
        private readonly ProdutoFornecedorService _produtoFornecedorService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ProdutosFornecedorController(
            ProdutoFornecedorService produtoFornecedorService,
            IHttpContextAccessor httpContextAccessor)
        {
            _produtoFornecedorService = produtoFornecedorService;
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpGet]
        public async Task<IActionResult> GetProdutosFornecedores()
        {
            var produtos = await _produtoFornecedorService.GetAllAsync();
            return Ok(produtos);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProdutoFornecedorById(int id)
        {
            var produto = await _produtoFornecedorService.GetByIdAsync(id);
            if (produto == null) return NotFound();
            return Ok(produto);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddProdutoFornecedor([FromBody] ProdutosFornecedorRequestDTO produtoDTO)
        {
            if (produtoDTO == null) return BadRequest();

            var novoProduto = await _produtoFornecedorService.CreateAsync(produtoDTO);
            return CreatedAtAction(nameof(GetProdutoFornecedorById), new { id = novoProduto.Id }, novoProduto);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateProdutoFornecedor(int id, [FromBody] ProdutoFornecedorDTO produtoDTO)
        {
            if (produtoDTO == null || id != produtoDTO.Id) return BadRequest();

            var produtoAtualizado = await _produtoFornecedorService.UpdateAsync(produtoDTO);
            if (produtoAtualizado == null) return NotFound();

            return Ok(produtoAtualizado);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteProdutoFornecedor(int id)
        {
            var produto = await _produtoFornecedorService.GetByIdAsync(id);
            if (produto == null) return NotFound();

            await _produtoFornecedorService.DeleteAsync(id);
            return NoContent();
        }
    }
}
