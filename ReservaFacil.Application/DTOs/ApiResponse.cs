using System;

namespace ReservaFacil.Application.DTOs;

public class ApiResponse<T>
{
    public bool Sucesso { get; set; } = true;
    public string Mensagem { get; set; } = string.Empty;
    public T Dados { get; set; } = default!;
    public int StatusCode { get; set; } = 200;
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;

    public static ApiResponse<T> Ok(T dados, string mensagem = "", int statusCode = 200)
    {
        return new ApiResponse<T>
        {
            Sucesso = true,
            Mensagem = mensagem,
            Dados = dados,
            Timestamp = DateTime.UtcNow,
            StatusCode = statusCode
        };
    }

    public static ApiResponse<T> Erro(string mensagem, int statusCode = 500)
    {
        return new ApiResponse<T>
        {
            Sucesso = false,
            Mensagem = mensagem,
            Timestamp = DateTime.UtcNow,
            StatusCode = statusCode
        };
    }
}
