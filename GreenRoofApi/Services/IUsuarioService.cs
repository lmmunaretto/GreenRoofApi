using GreenRoofApi.Data;
using GreenRoofApi.DTOs;
using GreenRoofApi.Models;
using Microsoft.EntityFrameworkCore;

namespace GreenRoofApi.Services
{
    public interface IUsuarioService
    {
        Task<List<UsuarioDTO>> GetAllAsync();

        Task<UsuarioDTO> GetByIdAsync(int id);


        Task CreateAsync(UsuarioDTO usuarioDTO);


        Task UpdateAsync(int id, UsuarioDTO usuarioDTO);


        Task DeleteAsync(int id);


        Task<UsuarioDTO> AuthenticateAsync(UsuarioLoginDTO usuarioLogin);


        Task<UsuarioResultDTO> RegisterAsync(UsuarioRegisterDTO usuarioDTO);
    }
}
