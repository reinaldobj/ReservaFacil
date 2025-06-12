using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using ReservaFacil.UnitTests.Integration;
using TechTalk.SpecFlow;
using ReservaFacil.API;
using ReservaFacil.Infrastructure.Data;
using ReservaFacil.Domain.Entities;
using ReservaFacil.Domain.Enums;
using ReservaFacil.Application.DTOs;
using ReservaFacil.Application.DTOs.Login;

namespace ReservaFacil.BddTests.Steps;

[Binding]
public class ApiSteps
{
    private readonly CustomWebApplicationFactory _factory;
    private readonly HttpClient _client;
    private HttpResponseMessage? _response;

    public ApiSteps()
    {
        _factory = new CustomWebApplicationFactory();
        _client = _factory.CreateClient();
    }

    private IServiceProvider Services => _factory.Services;

    [Given(@"que existe um usuário cadastrado com email ""(.*)"" e senha ""(.*)""")]
    public void GivenExistingUser(string email, string senha)
    {
        using var scope = Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<ReservaFacilDbContext>();
        db.Usuarios.Add(new Usuario
        {
            Id = Guid.NewGuid(),
            Nome = "User",
            Email = email,
            SenhaHash = BCrypt.Net.BCrypt.HashPassword(senha),
            TipoUsuario = TipoUsuario.UsuarioComum
        });
        db.SaveChanges();
    }

    [Given(@"que existe um espaço com ID (\d+) e nome ""(.*)""")]
    public void GivenExistingSpace(int id, string nome)
    {
        using var scope = Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<ReservaFacilDbContext>();
        var guid = IntToGuid(id);
        db.Espacos.Add(new Espaco
        {
            Id = guid,
            Nome = nome,
            Descricao = "desc",
            Capacidade = 10,
            TipoEspaco = TipoEspaco.Coworking,
            Disponivel = true
        });
        db.SaveChanges();
    }

    [Given(@"já existe um usuário com email ""(.*)""")]
    public void GivenExistingUserEmail(string email)
    {
        using var scope = Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<ReservaFacilDbContext>();
        db.Usuarios.Add(new Usuario
        {
            Id = Guid.NewGuid(),
            Nome = "User",
            Email = email,
            SenhaHash = BCrypt.Net.BCrypt.HashPassword("123456"),
            TipoUsuario = TipoUsuario.UsuarioComum
        });
        db.SaveChanges();
    }

    [Given(@"já existe uma reserva no espaço (\d+) entre ""(.*)"" e ""(.*)""")]
    public void GivenExistingReservation(int id, string inicio, string fim)
    {
        using var scope = Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<ReservaFacilDbContext>();
        var espacoId = IntToGuid(id);
        var usuario = new Usuario
        {
            Id = Guid.NewGuid(),
            Nome = "User",
            Email = $"u{Guid.NewGuid()}@test.com",
            SenhaHash = BCrypt.Net.BCrypt.HashPassword("123456"),
            TipoUsuario = TipoUsuario.UsuarioComum
        };
        db.Usuarios.Add(usuario);
        db.Reservas.Add(new Reserva
        {
            Id = Guid.NewGuid(),
            UsuarioId = usuario.Id,
            EspacoId = espacoId,
            DataInicio = DateTime.Parse(inicio),
            DataFim = DateTime.Parse(fim),
            StatusReserva = StatusReserva.Pendente
        });
        db.SaveChanges();
    }

    [Given(@"existe um espaço com ID (\d+)")]
    public void GivenSpaceWithId(int id)
    {
        using var scope = Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<ReservaFacilDbContext>();
        var guid = IntToGuid(id);
        if (!db.Espacos.Any(e => e.Id == guid))
        {
            db.Espacos.Add(new Espaco
            {
                Id = guid,
                Nome = $"Sala {id}",
                Descricao = "desc",
                Capacidade = 10,
                TipoEspaco = TipoEspaco.Coworking,
                Disponivel = true
            });
            db.SaveChanges();
        }
    }

    [Given(@"estou autenticado com token válido")]
    public async Task GivenAuthenticated()
    {
        var email = $"user{Guid.NewGuid()}@test.com";
        var password = "P@ssw0rd";
        using (var scope = Services.CreateScope())
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
        var login = await _client.PostAsJsonAsync("/api/Auth/login", new { Email = email, Senha = password });
        login.EnsureSuccessStatusCode();
        var data = await login.Content.ReadFromJsonAsync<ApiResponse<LoginOutputDto>>();
        var token = data!.Dados.Token;
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
    }

    [When(@"eu enviar um GET para ""(.*)""")]
    public async Task WhenGet(string url)
    {
        _response = await _client.GetAsync(url);
    }

    [When(@"eu enviar um POST para ""(.*)"" com o body:")]
    public async Task WhenPostWithBody(string url, string body)
    {
        _response = await _client.PostAsync(url, new StringContent(body, Encoding.UTF8, "application/json"));
    }

    [Then(@"o status da resposta deve ser (\d+)")]
    public void ThenStatusCode(int status)
    {
        ((int)_response!.StatusCode).Should().Be(status);
    }

    [Then(@"o corpo da resposta deve conter um campo ""(.*)"" não vazio")]
    public async Task ThenBodyHasField(string field)
    {
        var json = await _response!.Content.ReadAsStringAsync();
        using var doc = JsonDocument.Parse(json);
        var token = doc.RootElement.GetProperty("dados").GetProperty(field).GetString();
        token.Should().NotBeNullOrEmpty();
    }

    [Then(@"o corpo da resposta deve conter:")]
    public async Task ThenBodyContains(string expected)
    {
        var json = await _response!.Content.ReadAsStringAsync();
        json.Should().Contain(expected.Trim());
    }

    [Then(@"o corpo deve conter:")]
    public async Task ThenBodyContainsAlt(string expected)
    {
        await ThenBodyContains(expected);
    }

    [Then(@"o header ""(.*)"" deve apontar para ""(.*)""")]
    public void ThenHeaderLocation(string header, string value)
    {
        _response!.Headers.Location!.ToString().Should().Contain(value.Trim('{', '}'));
    }

    private static Guid IntToGuid(int id)
    {
        return new Guid($"00000000-0000-0000-0000-{id.ToString().PadLeft(12, '0')}");
    }
}
