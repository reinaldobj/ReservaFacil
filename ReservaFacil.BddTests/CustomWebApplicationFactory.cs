using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ReservaFacil.Infrastructure.Data;

namespace ReservaFacil.Tests.Integration
{
    public class CustomWebApplicationFactory : WebApplicationFactory<Program>
    {
                // Isso é executado antes do Program.CreateBuilder()
        protected override IHost CreateHost(IHostBuilder builder)
        {
            // 1) Garante que o IHostEnvironment.EnvironmentName == "Testing"
            builder.UseEnvironment("Testing");

            // 2) Agora o Program.cs, ao fazer builder.Environment.IsEnvironment("Testing"),
            //    vai cair na ramificação do InMemory e NÃO registrar o SQL Server.

            return base.CreateHost(builder);
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                var sp = services.BuildServiceProvider();
                using var scope = sp.CreateScope();
                var db = scope.ServiceProvider.GetRequiredService<ReservaFacilDbContext>();
                db.Database.EnsureDeleted();
                db.Database.EnsureCreated();
            });
        }
    }
}
