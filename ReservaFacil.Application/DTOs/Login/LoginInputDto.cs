using System;

namespace ReservaFacil.Application.DTOs.Login;

public class LoginInputDto
{
    public string Email { get; set; } = string.Empty;
    public string Senha { get; set; } = string.Empty;
}
