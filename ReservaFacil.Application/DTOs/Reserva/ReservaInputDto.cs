using System;
using ReservaFacil.Application.DTOs.Espaco;
using ReservaFacil.Application.DTOs.Usuario;

namespace ReservaFacil.Application.DTOs.Reserva;

public class ReservaInputDto
{
    public Guid UsuarioId { get; set; }
    public Guid EspacoId { get; set; }
    public DateTime DataInicio { get; set; }
    public DateTime DataFim { get; set; }
    public string StatusReserva { get; set; } = string.Empty; // 0 - Pendente, 1 - Confirmada, 2 - Cancelada
}
