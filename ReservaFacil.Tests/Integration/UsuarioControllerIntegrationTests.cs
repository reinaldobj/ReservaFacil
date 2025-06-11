using System;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;
using ReservaFacil.Application.DTOs.Usuario;
using ReservaFacil.Domain.Enums;
using Xunit;

namespace ReservaFacil.Tests.Integration;

public class UsuarioControllerIntegrationTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;
    private readonly CustomWebApplicationFactory _factory;

    public UsuarioControllerIntegrationTests(CustomWebApplicationFactory factory)
    {
        _factory = factory;
        _client = factory.CreateClient();
    }

    private async Task<string> GetAdminTokenAsync()
    {
        var email = $"admin{Guid.NewGuid()}@test.com";
        var password = "adminpass";
        return await TestHelper.CreateUserAndLoginAsync(_client, _factory.Services, email, password, TipoUsuario.Administrador);
    }

    [Fact]
    public async Task CriarEObterUsuario_Flow()
    {
        var createDto = new UsuarioInputDto { Nome = "Novo Usuario", Email = $"user{Guid.NewGuid()}@test.com", Senha = "123456", TipoUsuario = "UsuarioComum" };
        var createResponse = await _client.PostAsJsonAsync("/api/Usuario", createDto);
        createResponse.EnsureSuccessStatusCode();
        var created = await createResponse.Content.ReadFromJsonAsync<ApiResponse<UsuarioOutputDto>>();
        var id = created!.Dados.Id;

        var token = await GetAdminTokenAsync();
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        var getResponse = await _client.GetAsync($"/api/Usuario/{id}");
        getResponse.EnsureSuccessStatusCode();
    }
}
