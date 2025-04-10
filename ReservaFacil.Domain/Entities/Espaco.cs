using System;
using ReservaFacil.Domain.Enums;

namespace ReservaFacil.Domain.Entities;

public class Espaco
{
    public Guid Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string Descricao { get; set; } = string.Empty;
    public int Capacidade { get; set; }
    public TipoEspaco TipoEspaco { get; set; }
    public bool Disponivel {get;set;}
    public DateTime DataCriacao { get; set; } = DateTime.Now;
}
