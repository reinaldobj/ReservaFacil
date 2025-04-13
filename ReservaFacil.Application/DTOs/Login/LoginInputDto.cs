using System;
using System.ComponentModel.DataAnnotations;

namespace ReservaFacil.Application.DTOs.Login;

public class LoginInputDto
{
    [Required(ErrorMessage = "O campo Email é obrigatório.")]
    [EmailAddress(ErrorMessage = "O campo Email deve ser um endereço de e-mail válido.")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "O campo Senha é obrigatório.")]
    public string Senha { get; set; } = string.Empty;
}
