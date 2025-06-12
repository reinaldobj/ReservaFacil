using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using ReservaFacil.Application.DTOs;
using ReservaFacil.Application.DTOs.Espaco;
using ReservaFacil.Application.DTOs.Login;
using ReservaFacil.Domain.Entities;
using ReservaFacil.Domain.Enums;
using ReservaFacil.Infrastructure.Data;
using Xunit;

namespace ReservaFacil.UnitTests.Integration;

public class EspacoControllerIntegrationTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;
    private readonly CustomWebApplicationFactory _factory;

    public EspacoControllerIntegrationTests(CustomWebApplicationFactory factory)
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
    public async Task GetEspacoPorId_ReturnsOk()
    {
        Guid id;
        using (var scope = _factory.Services.CreateScope())
        {
            var db = scope.ServiceProvider.GetRequiredService<ReservaFacilDbContext>();
            var espaco = new Espaco
            {
                Id = Guid.NewGuid(),
                Nome = "Sala X",
                Descricao = "Desc",
                Capacidade = 5,
                TipoEspaco = TipoEspaco.Coworking,
                Disponivel = true
            };
            db.Espacos.Add(espaco);
            db.SaveChanges();
            id = espaco.Id;
        }

        var response = await _client.GetAsync($"/api/Espaco/{id}");
        response.EnsureSuccessStatusCode();
    }

    [Fact]
    public async Task CriarAtualizarDeletarEspaco_Flow()
    {
        var token = await GetAdminTokenAsync();
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var input = new EspacoInputDto { Nome = "SalaTest", Descricao = "Desc", Capacidade = 10, TipoEspaco = "SalaDeReuniao" };
        var createResponse = await _client.PostAsJsonAsync("/api/Espaco", input);
        Assert.Equal(HttpStatusCode.Created, createResponse.StatusCode);
        var created = await createResponse.Content.ReadFromJsonAsync<ApiResponse<EspacoOutputDto>>();
        var id = created!.Dados.Id;

        input.Nome = "SalaAtualizada";
        var updateResponse = await _client.PutAsJsonAsync($"/api/Espaco/{id}", input);
        updateResponse.EnsureSuccessStatusCode();

        var deleteResponse = await _client.DeleteAsync($"/api/Espaco/{id}");
        deleteResponse.EnsureSuccessStatusCode();
    }
}
