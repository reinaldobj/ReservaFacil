using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using AutoMapper;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using ReservaFacil.Application.DTOs.Login;
using ReservaFacil.Application.Interfaces;
using ReservaFacil.Domain.Entities;
using ReservaFacil.Infrastructure.Data.Repositories.Interfaces;

namespace ReservaFacil.Application.Services;

public class AuthService : IAuthService
{
    private readonly IUsuarioRepository _usuarioRepository;
    private readonly IConfiguration _configuration;

    private readonly IMapper _mapper;


    public AuthService(IUsuarioRepository usuarioRepository, IConfiguration configuration, IMapper mapper)
    {
        _usuarioRepository = usuarioRepository;
        _configuration = configuration;
        _mapper = mapper;
    }

    public LoginOutputDto Login(LoginInputDto loginInputDto)
    {
        var usuario = _usuarioRepository.ObterPorEmail(loginInputDto.Email);

        if (usuario == null)
            throw new Exception("Usuário não encontrado.");

        if (!BCrypt.Net.BCrypt.Verify(loginInputDto.Senha, usuario.SenhaHash))
            throw new Exception("Senha incorreta.");

        var token = GenerateToken(usuario);
        return new LoginOutputDto
        {
            Nome = usuario.Nome,
            Email = usuario.Email,
            TipoUsuario = usuario.TipoUsuario.ToString(),
            Token = token
        };
    }

    private string GenerateToken(Usuario usuario)
    {
        var chaveJwt = _configuration["Jwt:Key"] ?? throw new InvalidOperationException("Chave JWT não configurada.");
        var issuer = _configuration["Jwt:Issuer"] ?? throw new InvalidOperationException("Issuer JWT não configurado.");

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, usuario.Email),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(ClaimTypes.Role, usuario.TipoUsuario.ToString())
        };

        var chave = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(chaveJwt));
        var credenciais = new SigningCredentials(chave, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: issuer,
            audience: issuer,
            claims: claims,
            expires: DateTime.Now.AddHours(2),
            signingCredentials: credenciais
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
