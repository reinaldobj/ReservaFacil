using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using ReservaFacil.Domain.Entities;
using ReservaFacil.Domain.Enums;
using ReservaFacil.Infrastructure.Data;
using Xunit;

namespace ReservaFacil.Tests.Integration
{
    public class ApiTests : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly HttpClient _client;
        private readonly CustomWebApplicationFactory _factory;

        public ApiTests(CustomWebApplicationFactory factory)
        {
            _factory = factory;
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task GetEspacos_ReturnsOk()
        {
            using (var scope = _factory.Services.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<ReservaFacilDbContext>();
                db.Espacos.Add(new Espaco
                {
                    Id = Guid.NewGuid(),
                    Nome = "Teste",
                    Descricao = "Desc",
                    Capacidade = 1,
                    TipoEspaco = TipoEspaco.Auditorio,
                    Disponivel = true
                });
                db.SaveChanges();
            }

            var response = await _client.GetAsync("/api/Espaco");

            response.EnsureSuccessStatusCode();
        }
    }
}
