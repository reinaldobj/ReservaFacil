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
    private readonly ITokenService _tokenService;
    private readonly IMapper _mapper;


    public AuthService(IUsuarioRepository usuarioRepository, IConfiguration configuration, IMapper mapper, ITokenService tokenService)
    {
        _usuarioRepository = usuarioRepository;
        _configuration = configuration;
        _mapper = mapper;
        _tokenService = tokenService;
    }

    public LoginOutputDto? Login(LoginInputDto loginInputDto)
    {
        var usuario = _usuarioRepository.ObterPorEmail(loginInputDto.Email);

        if (usuario == null)
            return null;

        if (!BCrypt.Net.BCrypt.Verify(loginInputDto.Senha, usuario.SenhaHash))
            return null;

        var token = _tokenService.GerarToken(usuario);
        return new LoginOutputDto
        {
            Nome = usuario.Nome,
            Email = usuario.Email,
            TipoUsuario = usuario.TipoUsuario.ToString(),
            Token = token
        };
    }
}