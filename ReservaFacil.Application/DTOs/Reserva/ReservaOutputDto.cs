using System;
using ReservaFacil.Application.DTOs.Espaco;
using ReservaFacil.Application.DTOs.Usuario;

namespace ReservaFacil.Application.DTOs.Reserva;

public class ReservaOutputDto
{
    public Guid Id { get; set; }
    public EspacoOutputDto Espaco { get; set; }
    public UsuarioOutputDto Usuario { get; set; }
    public DateTime DataInicio { get; set; }
    public DateTime DataFim { get; set; }
    public String Status { get; set; }
}
