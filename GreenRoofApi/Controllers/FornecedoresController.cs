using GreenRoofApi.DTOs;
using GreenRoofApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Text.RegularExpressions;

namespace GreenRoofApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FornecedoresController : ControllerBase
    {
        private readonly FornecedorService _fornecedorService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public FornecedoresController(FornecedorService fornecedorService, IHttpContextAccessor httpContextAccessor)
        {
            _fornecedorService = fornecedorService;
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<FornecedorDTO>>> GetAll()
        {
            var fornecedores = await _fornecedorService.GetAllAsync();
            return Ok(fornecedores);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<FornecedorDTO>> GetById(int id)
        {
            var fornecedor = await _fornecedorService.GetByIdAsync(id);
            if (fornecedor == null)
            {
                return NotFound();
            }
            return Ok(fornecedor);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Create([FromBody] FornecedorRequestDTO fornecedorDTO)
        {
            var userId = _httpContextAccessor.HttpContext.User.FindFirstValue("usuarioId");
            var userRole = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Role);

            if (userRole != "Admin")
            {
                return Unauthorized("Apenas administradores podem cadastrar clientes.");
            }

            if (userId != null)
            {

                fornecedorDTO.AdminId = Int32.Parse(userId);
            }

            // Remover caracteres não numéricos do CNPJ
            fornecedorDTO.Cnpj = Regex.Replace(fornecedorDTO.Cnpj, "[^0-9]", "");

            // Verificar se o CNPJ possui 14 dígitos
            if (fornecedorDTO.Cnpj.Length != 14)
            {
                return BadRequest("O CNPJ deve conter 14 dígitos.");
            }


            var newFornecedor = await _fornecedorService.CreateAsync(fornecedorDTO);
            return CreatedAtAction(nameof(GetAll), new { id = newFornecedor.Id }, newFornecedor);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int id, [FromBody] FornecedorDTO fornecedorDTO)
        {
            await _fornecedorService.UpdateAsync(id, fornecedorDTO);
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            await _fornecedorService.DeleteAsync(id);
            return Ok();
        }
    }
}
