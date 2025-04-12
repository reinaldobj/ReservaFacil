using System;
using System.ComponentModel.DataAnnotations;

namespace ReservaFacil.Application.DTOs.Espaco;

public class EspacoOutputDto
{
    public Guid Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    
    public string Descricao { get; set; }

    public int Capacidade { get; set; }

    public string TipoEspaco { get; set; }

    public bool Disponivel { get; set; }

    public DateTime DataCriacao { get; set; }
}
