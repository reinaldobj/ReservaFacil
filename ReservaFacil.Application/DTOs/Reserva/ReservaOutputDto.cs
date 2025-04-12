using System;

namespace ReservaFacil.Application.DTOs.Reserva;

public class ReservaOutputDto
{
    public Guid Id { get; set; }
    public Guid EspacoId { get; set; }
    public Guid UsuarioId { get; set; }
    public DateTime DataInicio { get; set; }
    public DateTime DataFim { get; set; }
    public String Status { get; set; }
}
