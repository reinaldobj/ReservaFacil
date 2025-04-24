using System;

namespace ReservaFacil.Application.DTOs.Login;

public class LoginOutputDto
{
    public Guid Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string TipoUsuario { get; set; } = string.Empty; // 0 - Administrador, 1 - Usuario Comum
    public string Token { get; set; } = string.Empty;
}
