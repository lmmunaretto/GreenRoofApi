using GreenRoofApi.DTOs;
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
        public async Task<ActionResult> Register([FromBody] UsuarioRegisterDTO usuarioRegisterDTO)
        {
            var result = await _usuarioService.RegisterAsync(usuarioRegisterDTO);
            if (result == null)
            {
                return BadRequest(result);
            }
            return Ok("Usuário registrado com sucesso.");
        }

        [HttpPost("login")]
        public async Task<ActionResult<string>> Login([FromBody] UsuarioLoginDTO usuarioLoginDTO)
        {
            var token = await _usuarioService.AuthenticateAsync(usuarioLoginDTO);
            if (token == null)
            {
                return Unauthorized("Usuário ou senha inválidos.");
            }
            return Ok(token);
        }
    }
}
