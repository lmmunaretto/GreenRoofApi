using GreenRoofApi.DTOs;

namespace GreenRoofApi.Services
{
    public interface IUsuarioService
    {
        Task<List<UsuarioDTO>> GetAllAsync();

        Task<UsuarioDTO> GetByIdAsync(int id);


        Task CreateAsync(UsuarioDTO usuarioDTO);


        Task UpdateAsync(int id, UsuarioDTO usuarioDTO);


        Task DeleteAsync(int id);


        Task<UsuarioResultDTO> AuthenticateAsync(UsuarioLoginDTO usuarioLogin);


        Task<UsuarioResultDTO> RegisterAsync(UsuarioRegisterDTO usuarioDTO);
    }
}
