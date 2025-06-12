using System;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using ReservaFacil.Application.DTOs;
using ReservaFacil.Application.DTOs.Reserva;
using ReservaFacil.Application.DTOs.Espaco;
using ReservaFacil.Application.DTOs.Usuario;
using ReservaFacil.Domain.Entities;
using ReservaFacil.Domain.Enums;
using ReservaFacil.Infrastructure.Data;
using Xunit;

namespace ReservaFacil.UnitTests.Integration;

public class ReservaControllerIntegrationTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;
    private readonly CustomWebApplicationFactory _factory;

    public ReservaControllerIntegrationTests(CustomWebApplicationFactory factory)
    {
        _factory = factory;
        _client = factory.CreateClient();
    }

    private static DateTime GetValidStartDate()
    {
        var date = DateTime.Now.AddDays(3);
        while (date.DayOfWeek is DayOfWeek.Saturday or DayOfWeek.Sunday)
            date = date.AddDays(1);
        return new DateTime(date.Year, date.Month, date.Day, 9, 0, 0);
    }

    [Fact]
    public async Task CriarReserva_ReturnsCreated()
    {
        Guid userId;
        Guid espacoId;
        var start = GetValidStartDate();
        var end = start.AddHours(2);

        using (var scope = _factory.Services.CreateScope())
        {
            var db = scope.ServiceProvider.GetRequiredService<ReservaFacilDbContext>();
            var user = new Usuario
            {
                Id = Guid.NewGuid(),
                Nome = "Usu",
                Email = "u" + Guid.NewGuid() + "@test.com",
                SenhaHash = BCrypt.Net.BCrypt.HashPassword("123456"),
                TipoUsuario = TipoUsuario.UsuarioComum
            };
            db.Usuarios.Add(user);
            var espaco = new Espaco
            {
                Id = Guid.NewGuid(),
                Nome = "Sala R",
                Descricao = "Desc",
                Capacidade = 5,
                TipoEspaco = TipoEspaco.Coworking,
                Disponivel = true
            };
            db.Espacos.Add(espaco);
            db.SaveChanges();
            userId = user.Id;
            espacoId = espaco.Id;
        }

        var token = await TestHelper.CreateUserAndLoginAsync(_client, _factory.Services, "login" + Guid.NewGuid() + "@test.com", "123456", TipoUsuario.UsuarioComum);
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var dto = new ReservaInputDto
        {
            UsuarioId = userId,
            EspacoId = espacoId,
            DataInicio = start,
            DataFim = end,
            StatusReserva = "Pendente"
        };

        var response = await _client.PostAsJsonAsync("/api/Reserva", dto);
        Assert.Equal(System.Net.HttpStatusCode.Created, response.StatusCode);
    }
}
