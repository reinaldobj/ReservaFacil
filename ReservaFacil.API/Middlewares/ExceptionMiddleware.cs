using System.Text.Json;
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

    public async Task InvokeAsync(HttpContext context){
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro não tratado capturado pelo middleware.");

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

            context.Response.StatusCode = (int)statusCode;
            
            var response = new 
            {
                StatusCode = context.Response.StatusCode,
                Message = "Ocorreu um erro inesperado. Tente novamente mais tarde."
            };

            var json = JsonSerializer.Serialize(response);
            await context.Response.WriteAsync(json);

        }
    }
}
