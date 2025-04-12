using System;
using System.ComponentModel.DataAnnotations;

namespace ReservaFacil.Application.DTOs.Usuario;

public class UsuarioInputDto
{
    [Required]
    [StringLength(100, ErrorMessage = "O nome deve ter no máximo 100 caracteres.")]
    [MinLength(3, ErrorMessage = "O nome deve ter no mínimo 3 caracteres.")]
    public String Nome { get; set; }

    [Required]
    [EmailAddress(ErrorMessage = "O e-mail deve ser um endereço de e-mail válido.")]
    public String Email { get; set; } = string.Empty;

    [Required]
    [StringLength(100, ErrorMessage = "A senha deve ter no máximo 16 caracteres.")]
    [MinLength(6, ErrorMessage = "A senha deve ter no mínimo 6 caracteres.")]
    public String Senha { get; set; } = string.Empty;

    [Required]
    public String TipoUsuario { get; set; } = string.Empty; // 0 - Administrador, 1 - Usuario Comum
    public Guid Id { get; set; }
}
