using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.EntityFrameworkCore;
using ReservaFacil.API.Middlewares;
using ReservaFacil.Application.Interfaces;
using ReservaFacil.Application.Mappings;
using ReservaFacil.Application.Services;
using ReservaFacil.Infrastructure.Data;
using ReservaFacil.Infrastructure.Data.Repositories.Interfaces;
using ReservaFacil.Infrastructure.Data.Repositories.Repositories;
using Serilog;
using Serilog.Sinks.ApplicationInsights.TelemetryConverters;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var connection = String.Empty;

if(builder.Environment.IsDevelopment()){
    builder.Configuration.AddEnvironmentVariables().AddJsonFile("appsettings.Development.json");
    connection = builder.Configuration.GetConnectionString("DefaultConnection");
}
else{
    connection = builder.Configuration.GetConnectionString("DefaultConnection");
}

builder.Services.AddDbContext<ReservaFacilDbContext>(options =>
    options.UseSqlServer(connection));

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddScoped<IEspacoRepository, EspacoRepository>();
builder.Services.AddScoped<IEspacoService, EspacoService>();

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

app.UseExceptionHandlerMiddleware();
app.MapControllers();

app.Run();