using GreenRoofApi.Models;

namespace GreenRoofApi.Services
{
    public interface ITokenService
    {
        Task<string> GenerateTokenAsync(Usuario usuario);
    }
}
