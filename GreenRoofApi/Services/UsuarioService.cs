using GreenRoofApi.Data;
using GreenRoofApi.DTOs;
using GreenRoofApi.Models;
using Microsoft.EntityFrameworkCore;

namespace GreenRoofApi.Services
{
    public class UsuarioService : IUsuarioService
    {
        private readonly GreenRoofContext _context;
        private readonly TokenService _tokenService;

        public UsuarioService(GreenRoofContext context, TokenService tokenService)
        {
            _context = context;
            _tokenService = tokenService;
        }

        public async Task<List<UsuarioDTO>> GetAllAsync()
        {
            var usuarios = await _context.Usuarios.ToListAsync();
            return usuarios.Select(u => new UsuarioDTO
            {
                Id = u.Id,
                Nome = u.Nome,
                Email = u.Email,
                Senha = u.Senha,
                Role = u.Role
            }).ToList();
        }

        public async Task<UsuarioDTO> GetByIdAsync(int id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario == null) return null;

            return new UsuarioDTO
            {
                Id = usuario.Id,
                Nome = usuario.Nome,
                Email = usuario.Email,
                Senha = usuario.Senha,
                Role = usuario.Role
            };
        }

        public async Task CreateAsync(UsuarioDTO usuarioDTO)
        {
            var usuario = new Usuario
            {
                Nome = usuarioDTO.Nome,
                Email = usuarioDTO.Email,
                Senha = usuarioDTO.Senha,
                Role = usuarioDTO.Role
            };

            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(int id, UsuarioDTO usuarioDTO)
        {
            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario == null) return;

            usuario.Nome = usuarioDTO.Nome;
            usuario.Email = usuarioDTO.Email;
            usuario.Senha = usuarioDTO.Senha;
            usuario.Role = usuarioDTO.Role;

            _context.Usuarios.Update(usuario);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario == null) return;

            _context.Usuarios.Remove(usuario);
            await _context.SaveChangesAsync();
        }

        public async Task<UsuarioResultDTO> AuthenticateAsync(UsuarioLoginDTO usuarioLogin)
        {
            string email = usuarioLogin.Email;
            string senha = usuarioLogin.Senha;

            var usuario = await _context.Usuarios.SingleOrDefaultAsync(u => u.Email == email && u.Senha == senha);
            if (usuario == null) return null;

            var token = await _tokenService.GenerateTokenAsync(usuario);

            return new UsuarioResultDTO
            {
                Succeeded = true,
                Token = token
            };
        }

        public async Task<UsuarioResultDTO> RegisterAsync(UsuarioRegisterDTO usuarioDTO)
        {
            // Verificar se o e-mail já está em uso
            var existingUser = await _context.Usuarios.SingleOrDefaultAsync(u => u.Email == usuarioDTO.Email);
            if (existingUser != null)
            {
                return new UsuarioResultDTO
                {
                    Succeeded = false,
                    Errors = new[] { "Email já está em uso." }

                };
            }

            // Criar novo usuário
            var novoUsuario = new Usuario
            {
                Nome = usuarioDTO.Nome,
                Email = usuarioDTO.Email,
                Senha = usuarioDTO.Senha, // Aqui você pode adicionar a lógica de criptografia da senha
                Role = usuarioDTO.Role,
            };

            novoUsuario.Role = char.ToUpper(novoUsuario.Role[0]) + novoUsuario.Role.Substring(1).ToLower();

            _context.Usuarios.Add(novoUsuario);
            await _context.SaveChangesAsync();


            // Gerar o token JWT após o registro bem-sucedido
            var token = await _tokenService.GenerateTokenAsync(novoUsuario);

            return new UsuarioResultDTO
            {
                Succeeded = true,
                Token = token
            };
        }
    }
}
