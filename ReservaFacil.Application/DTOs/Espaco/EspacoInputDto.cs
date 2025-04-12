using System;
using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography.X509Certificates;

namespace ReservaFacil.Application.DTOs.Espaco;

public class EspacoInputDto
{
    [Required]
    [StringLength(100, ErrorMessage = "O nome não pode ter mais de 100 caracteres.")]
    public string Nome { get; set; } = string.Empty;
    
    [Required]
    [StringLength(500, ErrorMessage = "A descrição não pode ter mais de 500 caracteres.")]
    public string Descricao { get; set; }

    [Range(1, 100)]
    [Required]
    public int Capacidade { get; set; }

    [Required]
    public string TipoEspaco { get; set; }

    public bool Disponivel { get; set; } = true;
    
    public Guid Id { get; set; }
}
