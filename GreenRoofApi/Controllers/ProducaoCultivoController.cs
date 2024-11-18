using GreenRoofApi.DTOs;
using GreenRoofApi.Models;
using GreenRoofApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace GreenRoofApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProducaoCultivoController : ControllerBase
    {
        private readonly ProducaoCultivoService _producaoCultivoService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ProducaoCultivoController(ProducaoCultivoService producaoCultivoService, IHttpContextAccessor httpContextAccessor)
        {
            _producaoCultivoService = producaoCultivoService;
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddProducao([FromBody] ProducaoCultivoRequestDTO producaoRequest)
        {
            var adminId = _httpContextAccessor.HttpContext.User.FindFirstValue("usuarioId");
            if (adminId == null) return Unauthorized();

            var novaProducao = await _producaoCultivoService.CreateAsync(producaoRequest, int.Parse(adminId));
            return CreatedAtAction(nameof(GetProducaoById), new { id = novaProducao.Id }, novaProducao);
        }

        [HttpGet]
        [Authorize(Roles = "Admin, Cliente")]
        public async Task<IActionResult> GetProducoes()
        {
            var producoes = await _producaoCultivoService.GetAllAsync();
            return Ok(producoes);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Admin, Cliente")]
        public async Task<IActionResult> GetProducaoById(int id)
        {
            var producao = await _producaoCultivoService.GetByIdAsync(id);
            if (producao == null) return NotFound();
            return Ok(producao);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateProducao(int id, [FromBody] ProducaoCultivoDTO producaoRequest)
        {
            var adminId = _httpContextAccessor.HttpContext.User.FindFirstValue("usuarioId");
            if (adminId == null) return Unauthorized();

            var updatedProducao = await _producaoCultivoService.UpdateAsync(id, producaoRequest, int.Parse(adminId));
            if (updatedProducao == null) return NotFound();
            return Ok(updatedProducao);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteProducao(int id)
        {
            var success = await _producaoCultivoService.DeleteAsync(id);
            if (!success) return NotFound();
            return NoContent();
        }
    }
}
