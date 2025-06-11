using System;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using ReservaFacil.Application.DTOs;
using ReservaFacil.Application.DTOs.Login;
using ReservaFacil.Domain.Entities;
using ReservaFacil.Domain.Enums;
using ReservaFacil.Infrastructure.Data;
using Xunit;

namespace ReservaFacil.Tests.Integration;

public class AuthControllerIntegrationTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;
    private readonly CustomWebApplicationFactory _factory;

    public AuthControllerIntegrationTests(CustomWebApplicationFactory factory)
    {
        _factory = factory;
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task Login_ReturnsToken()
    {
        var email = $"user{Guid.NewGuid()}@test.com";
        var password = "123456";
        using (var scope = _factory.Services.CreateScope())
        {
            var db = scope.ServiceProvider.GetRequiredService<ReservaFacilDbContext>();
            db.Usuarios.Add(new Usuario
            {
                Id = Guid.NewGuid(),
                Nome = "User",
                Email = email,
                SenhaHash = BCrypt.Net.BCrypt.HashPassword(password),
                TipoUsuario = TipoUsuario.UsuarioComum
            });
            db.SaveChanges();
        }

        var response = await _client.PostAsJsonAsync("/api/Auth/login", new { Email = email, Senha = password });
        response.EnsureSuccessStatusCode();
        var data = await response.Content.ReadFromJsonAsync<ApiResponse<LoginOutputDto>>();
        Assert.False(string.IsNullOrEmpty(data!.Dados.Token));
    }
}
