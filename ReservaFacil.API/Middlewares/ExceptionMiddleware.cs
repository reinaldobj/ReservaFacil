using System.Text.Json;
using ReservaFacil.Application.DTOs;
using ReservaFacil.Domain.Exceptions;

namespace ReservaFacil.API.Middlewares;

public class ExceptionMiddleware
{
    private readonly ILogger<ExceptionMiddleware> _logger;
    private readonly RequestDelegate _next;

    public ExceptionMiddleware(ILogger<ExceptionMiddleware> logger, RequestDelegate next)
    {
        _logger = logger;
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            var traceId = context.TraceIdentifier;

            _logger.LogError(ex, $"Erro não tratado capturado pelo middleware. TraceId: {traceId}");

            context.Response.ContentType = "application/json";

            var statusCode = ex switch
            {
                InvalidOperationException => StatusCodes.Status400BadRequest,
                ArgumentNullException => StatusCodes.Status400BadRequest,
                ArgumentException => StatusCodes.Status400BadRequest,
                FormatException => StatusCodes.Status400BadRequest,
                KeyNotFoundException => StatusCodes.Status404NotFound,
                ValidationException => StatusCodes.Status400BadRequest,
                BusinessException => StatusCodes.Status400BadRequest,
                NotFoundException => StatusCodes.Status404NotFound,
                _ => StatusCodes.Status500InternalServerError
            };

            var mensagem = statusCode switch
            {
                StatusCodes.Status400BadRequest => ex.Message,
                StatusCodes.Status404NotFound => ex.Message,
                _ => "Desculpe, algo deu errado. Por favor, tente novamente mais tarde."
            };

            var response = ApiResponse<string>.Erro(mensagem, statusCode);

            context.Response.StatusCode = (int)statusCode;
            context.Response.Headers.Append("X-Trace-Id", traceId);

            var json = JsonSerializer.Serialize(response);
            await context.Response.WriteAsync(json);

        }
    }
}
