using System;

namespace ReservaFacil.Application.DTOs.Usuario;

public class UsuarioOutputDto
{
    public Guid Id { get; set; }

    public String Nome { get; set; }

    public String Email { get; set; } = string.Empty;

    public String TipoUsuario { get; set; } = string.Empty; // 0 - Administrador, 1 - Usuario Comum
}
