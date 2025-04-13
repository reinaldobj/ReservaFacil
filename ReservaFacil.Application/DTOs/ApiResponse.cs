using System;

namespace ReservaFacil.Application.DTOs;

public class ApiResponse<T>
{
    public bool Sucesso { get; set; } = true;
    public string Mensagem { get; set; } = string.Empty;
    public T Dados { get; set; } = default!;
    public int StatusCode { get; set; } = 200;
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;

    public static ApiResponse<T> Ok(T dados, string mensagem = "")
    {
        return new ApiResponse<T>
        {
            Sucesso = true,
            Mensagem = mensagem,
            Dados = dados,
            Timestamp = DateTime.UtcNow
        };
    }

    public static ApiResponse<T> Erro(string mensagem)
    {
        return new ApiResponse<T>
        {
            Sucesso = false,
            Mensagem = mensagem,
            Timestamp = DateTime.UtcNow
        };
    }
}
