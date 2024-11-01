using GreenRoofApi.DTOs;
using GreenRoofApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GreenRoofApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuariosController : ControllerBase
    {
        private readonly UsuarioService _usuarioService;

        public UsuariosController(UsuarioService usuarioService)
        {
            _usuarioService = usuarioService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UsuarioRegisterDTO usuarioDTO)
        {
            var result = await _usuarioService.RegisterAsync(usuarioDTO);
            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            return Ok(new { result.Token });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UsuarioLoginDTO loginDTO)
        {
            var token = await _usuarioService.AuthenticateAsync(loginDTO);
            if (token == null)
            {
                return Unauthorized("Usuário ou senha inválidos.");
            }

            return Ok(new { token });
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllUsuarios()
        {
            var usuarios = await _usuarioService.GetAllAsync();
            return Ok(usuarios);
        }
    }
}
