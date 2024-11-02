using GreenRoofApi.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

public class TokenService
{
    private readonly IConfiguration _config;

    public TokenService(IConfiguration config)
    {
        _config = config;
    }

    public async Task<string> GenerateTokenAsync(Usuario usuario)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        usuario.Role = char.ToUpper(usuario.Role[0]) + usuario.Role.Substring(1).ToLower();

        var claims = new[]
        {
        new Claim(JwtRegisteredClaimNames.Sub, usuario.Nome),
        new Claim(JwtRegisteredClaimNames.Email, usuario.Email),
        new Claim("role", usuario.Role),
        new Claim("usuarioId", usuario.Id.ToString())
    };

        var token = new JwtSecurityToken(
            _config["Jwt:Issuer"],
            _config["Jwt:Audience"],
            claims,
            expires: DateTime.Now.AddHours(1),
            signingCredentials: creds
        );

        return await Task.FromResult(tokenHandler.WriteToken(token));
    }


}

