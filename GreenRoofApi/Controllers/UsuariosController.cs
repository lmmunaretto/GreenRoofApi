﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using GreenRoofApi.DTOs;
using GreenRoofApi.Services;

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

        // Registro de novo usuário
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

        // Login de usuário
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

        // Listar usuários (apenas Admin)
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllUsuarios()
        {
            var usuarios = await _usuarioService.GetAllAsync();
            return Ok(usuarios);
        }
    }
}
