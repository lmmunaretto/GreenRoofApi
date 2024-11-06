using GreenRoofApi.DTOs;
using GreenRoofApi.Models;
using GreenRoofApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Text.RegularExpressions;

namespace GreenRoofAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientesController : ControllerBase
    {
        private readonly ClienteService _clienteService;
        private readonly UsuarioService _usuarioService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ClientesController(ClienteService clienteService, IHttpContextAccessor httpContextAccessor, UsuarioService usuarioService)
        {
            _clienteService = clienteService;
            _httpContextAccessor = httpContextAccessor;
            _usuarioService = usuarioService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ClienteDTO>>> GetAll()
        {
            var clientes = await _clienteService.GetAllAsync();
            return Ok(clientes);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ClienteDTO>> GetById(int id)
        {
            var cliente = await _clienteService.GetByIdAsync(id);
            if (cliente == null)
            {
                return NotFound();
            }
            return Ok(cliente);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Create([FromBody] ClienteRequestDTO clienteDTO)
        {
            var userId = _httpContextAccessor.HttpContext.User.FindFirstValue("usuarioId");
            var userRole = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Role);

            // Verificar permissões e lógica do cadastro
            if (userRole != "Admin")
            {
                return Unauthorized("Apenas administradores podem cadastrar clientes.");
            }

            if(userId != null)
            {

                    clienteDTO.AdminId = int.Parse(userId);
            }

            clienteDTO.Cpf = Regex.Replace(clienteDTO.Cpf, "[^0-9]", "");

            // Verificar se o CPF possui 11 dígitos
            if (clienteDTO.Cpf.Length != 11)
            {
                return BadRequest("O CPF deve conter 11 dígitos.");
            }

            var newCliente = await _clienteService.CreateAsync(clienteDTO);


            string senhaTemporaria = GerarSenhaAleatoria();
            var usuario = new UsuarioDTO
            {
                Nome = clienteDTO.Nome,
                Email = clienteDTO.Email,
                Senha = senhaTemporaria,
                Role = "Cliente",
                DeveTrocarSenha = true
            };

            await _usuarioService.CreateAsync(usuario);

            return CreatedAtAction(nameof(GetById), new { id = newCliente.Id }, new { Cliente = newCliente, SenhaTemporaria = senhaTemporaria });

        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int id, [FromBody] ClienteDTO clienteDTO)
        {
            await _clienteService.UpdateAsync(id, clienteDTO);
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            await _clienteService.DeleteAsync(id);
            return Ok();
        }

        private string GerarSenhaAleatoria()
        {
            const string caracteres = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var random = new Random();
            return new string(Enumerable.Repeat(caracteres, 8)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}
