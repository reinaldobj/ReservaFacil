using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ReservaFacil.Infrastructure.Data;

namespace ReservaFacil.Tests.Integration
{
    public class CustomWebApplicationFactory : WebApplicationFactory<Program>
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.UseEnvironment("Development");

            builder.ConfigureAppConfiguration((context, configBuilder) =>
            {
                configBuilder.AddInMemoryCollection(new Dictionary<string, string>
                {
                    {"Jwt:Key", "TestKey123456789012345678901234567890"},
                    {"Jwt:Issuer", "TestIssuer"}
                });
            });

            builder.ConfigureServices(services =>
            {
                var descriptor = services.SingleOrDefault(
                    d => d.ServiceType == typeof(DbContextOptions<ReservaFacilDbContext>));
                if (descriptor != null)
                    services.Remove(descriptor);

                services.AddDbContext<ReservaFacilDbContext>(options =>
                {
                    options.UseInMemoryDatabase("InMemoryTest" + Guid.NewGuid());
                });

                var sp = services.BuildServiceProvider();
                using var scope = sp.CreateScope();
                var db = scope.ServiceProvider.GetRequiredService<ReservaFacilDbContext>();
                db.Database.EnsureCreated();
            });
        }
    }
}
