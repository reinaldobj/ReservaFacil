using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.InMemory;
using ReservaFacil.API.Middlewares;
using ReservaFacil.Application.Interfaces;
using ReservaFacil.Application.Mappings;
using ReservaFacil.Application.Services;
using ReservaFacil.Infrastructure.Data;
using ReservaFacil.Infrastructure.Data.Repositories.Interfaces;
using ReservaFacil.Infrastructure.Data.Repositories.Repositories;
using Serilog;
using Serilog.Sinks.ApplicationInsights.TelemetryConverters;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options => 
{
    options.SwaggerDoc("v1", new() { Title = "Reserva Fácil API", Version = "v1" });

    options.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme{
        Name = "Authorization",
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Description = "Insira o Token JWT assim: **Bearer {seu token}**"
    });

    options.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement{
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme{
                Reference = new Microsoft.OpenApi.Models.OpenApiReference{
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

var connection = String.Empty;

if (builder.Environment.IsDevelopment()){
    builder.Configuration
    .AddUserSecrets<Program>()
    .AddEnvironmentVariables()
    .AddJsonFile("appsettings.Development.json", optional: true, reloadOnChange: true);

    connection = builder.Configuration.GetConnectionString("DefaultConnection");

    builder.Services.AddDbContext<ReservaFacilDbContext>(opts =>
    opts.UseSqlServer(connection));
}
else if (builder.Environment.IsEnvironment("Testing"))
{
    connection = "DataSource=:memory:";

    builder.Services.AddDbContext<ReservaFacilDbContext>(opts =>
    opts.UseInMemoryDatabase("ReservaFacil_TestDb"));
}
else
{
    connection = Environment.GetEnvironmentVariable("SQL_CONNECTION_STRING") ?? throw new InvalidOperationException("String de conexão não configurada.");

    builder.Services.AddDbContext<ReservaFacilDbContext>(opts =>
        opts.UseSqlServer(connection));
}

builder.Services.AddAutoMapper(typeof(Program).Assembly);

builder.Services.AddAutoMapper(
    cfg => {
       // configurações adicionais, se houver  
    },
    // lista só seus assemblies de profiles
    typeof(EspacoProfile).Assembly,
    typeof(UsuarioProfile).Assembly,
    typeof(ReservaProfile).Assembly
);

builder.Services.AddScoped<IEspacoRepository, EspacoRepository>();
builder.Services.AddScoped<IEspacoService, EspacoService>();
builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();
builder.Services.AddScoped<IUsuarioService, UsuarioService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IReservaRepository, ReservaRepository>();
builder.Services.AddScoped<IReservaService, ReservaService>();
builder.Services.AddScoped<ITokenService, TokenService>();

var jwtKey = builder.Configuration["Jwt:Key"] ?? throw new InvalidOperationException("Chave JWT não configurada.");
var jwtIssuer = builder.Configuration["Jwt:Issuer"] ?? throw new InvalidOperationException("Issuer JWT não configurado.");

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtIssuer,
        ValidAudience = jwtIssuer,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
    };
});

builder.Services.AddControllers();

var appInsightConnectionString = builder.Configuration["ApplicationInsights:ConnectionString"];

Log.Logger = new LoggerConfiguration()
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .WriteTo.ApplicationInsights(
        connectionString: appInsightConnectionString, 
        telemetryConverter: new TraceTelemetryConverter()
)
    .CreateLogger();

builder.Host.UseSerilog();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.UseExceptionHandlerMiddleware();
app.MapControllers();

app.Run();

public partial class Program { }
