using System;
using System.ComponentModel.DataAnnotations.Schema;
using ReservaFacil.Domain.Enums;

namespace ReservaFacil.Domain.Entities;

public class Reserva
{
    public Guid Id { get; set; }
    public Guid UsuarioId { get; set; }
    public Guid EspacoId { get; set; }
    public DateTime DataInicio { get; set; }
    public DateTime DataFim { get; set; }
    public StatusReserva StatusReserva{ get; set; } // 0 - Pendente, 1 - Confirmada, 2 - Cancelada
    public DateTime DataCriacao { get; set; } = DateTime.Now;
    public Espaco Espaco {get; set; }
    public Usuario Usuario { get; set; }
}
