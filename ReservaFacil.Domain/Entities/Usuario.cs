using System;
using System.ComponentModel.DataAnnotations.Schema;
using ReservaFacil.Domain.Enums;

namespace ReservaFacil.Domain.Entities;

public class Usuario
{
    public Guid Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string SenhaHash { get; set; } = string.Empty;
    public TipoUsuario TipoUsuario {get; set;} // 0 - Administrador, 1 - Usuario Comum
    public DateTime DataCriacao { get; set; } = DateTime.Now;
}
