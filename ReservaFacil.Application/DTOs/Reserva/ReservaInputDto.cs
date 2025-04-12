using System;
using ReservaFacil.Application.DTOs.Espaco;
using ReservaFacil.Application.DTOs.Usuario;

namespace ReservaFacil.Application.DTOs.Reserva;

public class ReservaInputDto
{
    public UsuarioInputDto Usuario { get; set; } = new UsuarioInputDto();
    public EspacoInputDto Espaco { get; set; } = new EspacoInputDto();
    public DateTime DataInicio { get; set; }
    public DateTime DataFim { get; set; }
    public DateTime DataReserva { get; set; } = DateTime.Now;
    public string StatusReserva { get; set; } = string.Empty; // 0 - Pendente, 1 - Confirmada, 2 - Cancelada
}
