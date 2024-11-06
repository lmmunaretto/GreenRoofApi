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
            var usuarioCriado = await _usuarioService.RegisterAsync(usuarioDTO);
            if (!usuarioCriado.Succeeded)
            {
                return BadRequest(usuarioCriado.Errors);
            }

            return Ok(new { usuarioCriado.Token });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UsuarioLoginDTO loginDTO)
        {
            var usuarioAutenticado = await _usuarioService.AuthenticateAsync(loginDTO);

            if (!usuarioAutenticado.Succeeded)
            {
                return Unauthorized(usuarioAutenticado.Errors);
            }

            return Ok(usuarioAutenticado);
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllUsuarios()
        {
            var usuarios = await _usuarioService.GetAllAsync();
            return Ok(usuarios);
        }

        [HttpPost("trocar-senha")]
        public async Task<IActionResult> TrocarSenha(TrocaSenhaRequest request)
        {
            var usuarios = await _usuarioService.GetAllAsync();
            var usuario = usuarios.Where(x => x.Email == request.Email).FirstOrDefault();

            if (request.SenhaAtual != usuario.Senha)
            {
                return BadRequest("Senha atual incorreta.");
            }

            usuario.Senha = request.NovaSenha;
            usuario.DeveTrocarSenha = false;

            await _usuarioService.UpdateAsync(usuario);
            return Ok("Senha trocada com sucesso.");
        }
    }
}
