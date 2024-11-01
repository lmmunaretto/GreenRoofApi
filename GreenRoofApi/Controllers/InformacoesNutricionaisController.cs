using GreenRoofApi.DTOs;
using GreenRoofApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GreenRoofApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InformacoesNutricionaisController : ControllerBase
    {
        private readonly InformacaoNutricionalService _informacaoService;

        public InformacoesNutricionaisController(InformacaoNutricionalService informacaoService)
        {
            _informacaoService = informacaoService;
        }

        // Listar informações nutricionais (aberto)
        [HttpGet]
        public async Task<IActionResult> GetInformacoesNutricionais()
        {
            var informacoes = await _informacaoService.GetAllAsync();

            return Ok(informacoes);
        }

        // Adicionar informação nutricional (apenas Admin)
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddInformacaoNutricional([FromBody] InformacaoNutricionalRequestDTO informacao)
        {
            var newInformacao = await _informacaoService.CreateAsync(informacao);
            return CreatedAtAction(nameof(GetInformacoesNutricionais), new { id = newInformacao.Id }, newInformacao);
        }

        // Atualizar informação nutricional (apenas Admin)
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateInformacaoNutricional(int id, [FromBody] InformacaoNutricionalDTO informacao)
        {
            if (id != informacao.Id)
                return BadRequest();

            var updatedInformacao = await _informacaoService.UpdateAsync(id, informacao);
            return Ok(updatedInformacao);
        }

        // Deletar informação nutricional (apenas Admin)
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteInformacaoNutricional(int id)
        {
            await _informacaoService.DeleteAsync(id);

            return NoContent();
        }
    }
}
