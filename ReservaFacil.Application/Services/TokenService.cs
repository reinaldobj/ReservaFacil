using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using ReservaFacil.Application.Interfaces;
using ReservaFacil.Domain.Entities;

namespace ReservaFacil.Application.Services;

public class TokenService : ITokenService
{
    private readonly IConfiguration _configuration;

    public TokenService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public string GerarToken(Usuario usuario)
    {
        var chaveJwt = _configuration["Jwt:Key"] ?? throw new InvalidOperationException("Chave JWT não configurada.");
        var issuer = _configuration["Jwt:Issuer"] ?? throw new InvalidOperationException("Issuer JWT não configurado.");

        var claims = new[]
        {
            // Sub (Subject) agora utiliza o identificador unico do usuario para
            // facilitar a validacao de autorizacao.
            new Claim(JwtRegisteredClaimNames.Sub, usuario.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(ClaimTypes.Role, usuario.TipoUsuario.ToString())
        };

        var chave = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(chaveJwt));
        var credenciais = new SigningCredentials(chave, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: issuer,
            audience: issuer,
            claims: claims,
            expires: DateTime.UtcNow.AddHours(2),
            signingCredentials: credenciais
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}