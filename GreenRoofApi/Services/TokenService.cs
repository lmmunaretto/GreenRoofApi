using GreenRoofApi.Models;
using GreenRoofApi.Services;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

public class TokenService
{
    private readonly IConfiguration _config;
    private readonly ClienteService _clienteService;

    public TokenService(IConfiguration config, ClienteService clienteService)
    {
        _config = config;
        _clienteService = clienteService;
    }

    public async Task<string> GenerateTokenAsync(Usuario usuario)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var clienteID = "";
        var usuarioId = usuario.Id;

        usuario.Role = char.ToUpper(usuario.Role[0]) + usuario.Role.Substring(1).ToLower();

        if (usuario.Role == "Cliente")
        {
            var clientes = await _clienteService.GetAllAsync();
            var clienteObj = clientes.Where(c => c.Email == usuario.Email).FirstOrDefault();

            if (clienteObj != null)
            {
                clienteID = clienteObj.Id.ToString();
                usuarioId = clienteObj.AdminId;
            }
        }

        var claims = new[]
        {
        new Claim(JwtRegisteredClaimNames.Sub, usuario.Nome),
        new Claim(JwtRegisteredClaimNames.Email, usuario.Email),
        new Claim("role", usuario.Role),
        new Claim("usuarioId", usuarioId.ToString()),
        new Claim("deveTrocarSenha", usuario.DeveTrocarSenha.ToString()),
        new Claim("clienteId", clienteID)

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

