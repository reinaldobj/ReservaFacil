using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using ReservaFacil.Application.DTOs;
using ReservaFacil.Application.DTOs.Login;
using ReservaFacil.Domain.Entities;
using ReservaFacil.Domain.Enums;
using ReservaFacil.Infrastructure.Data;

namespace ReservaFacil.Tests.Integration;

internal static class TestHelper
{
    public static async Task<string> CreateUserAndLoginAsync(HttpClient client, IServiceProvider services, string email, string password, TipoUsuario tipo)
    {
        using var scope = services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<ReservaFacilDbContext>();
        db.Usuarios.Add(new Usuario
        {
            Id = Guid.NewGuid(),
            Nome = "TestUser",
            Email = email,
            SenhaHash = BCrypt.Net.BCrypt.HashPassword(password),
            TipoUsuario = tipo
        });
        db.SaveChanges();

        var response = await client.PostAsJsonAsync("/api/Auth/login", new { Email = email, Senha = password });
        response.EnsureSuccessStatusCode();
        var apiResponse = await response.Content.ReadFromJsonAsync<ApiResponse<LoginOutputDto>>();
        return apiResponse!.Dados.Token;
    }
}
