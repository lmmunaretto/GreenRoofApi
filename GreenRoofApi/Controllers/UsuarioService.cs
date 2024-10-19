using GreenRoofApi.Data;
using GreenRoofApi.DTOs;
using GreenRoofApi.Models;

namespace GreenRoofApi.Controllers
{
    public class UsuarioService
    {
        private readonly GreenRoofContext _context;

        public UsuarioService(GreenRoofContext context)
        {
            _context = context;
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

        public async Task<UsuarioDTO> AuthenticateAsync(UsuarioLoginDTO usuarioLogin)
        {
            string email = usuarioLogin.Email;
            string senha = usuarioLogin.Senha;

            var usuario = await _context.Usuarios.SingleOrDefaultAsync(u => u.Email == email && u.Senha == senha);
            if (usuario == null) return null;

            return new UsuarioDTO
            {
                Id = usuario.Id,
                Nome = usuario.Nome,
                Email = usuario.Email,
                Role = usuario.Role
            };
        }

        public async Task<UsuarioDTO> RegisterAsync(UsuarioRegisterDTO usuarioDTO)
        {
            // Verificar se o e-mail já está em uso
            var existingUser = await _context.Usuarios.SingleOrDefaultAsync(u => u.Email == usuarioDTO.Email);
            if (existingUser != null)
            {
                return null; // Já existe um usuário com este e-mail
            }

            // Criar novo usuário
            var novoUsuario = new Usuario
            {
                Nome = usuarioDTO.Nome,
                Email = usuarioDTO.Email,
                Senha = usuarioDTO.Senha, // Aqui você pode adicionar a lógica de criptografia da senha
                Role = usuarioDTO.Role
            };

            _context.Usuarios.Add(novoUsuario);
            await _context.SaveChangesAsync();

            return new UsuarioDTO
            {
                Id = novoUsuario.Id,
                Nome = novoUsuario.Nome,
                Email = novoUsuario.Email,
                Role = novoUsuario.Role
            };
        }
    }
}
