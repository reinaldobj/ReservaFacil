using System;
using System.ComponentModel.DataAnnotations;
using ReservaFacil.Application.DTOs.Espaco;
using ReservaFacil.Application.DTOs.Usuario;

namespace ReservaFacil.Application.DTOs.Reserva;

public class ReservaInputDto
{
    [Required(ErrorMessage = "O campo UsuarioId é obrigatório.")]
    public Guid UsuarioId { get; set; }
    [Required(ErrorMessage = "O campo EspacoId é obrigatório.")]
    public Guid EspacoId { get; set; }
    [Required(ErrorMessage = "O campo DataInicio é obrigatório.")]
    [DataType(DataType.Date, ErrorMessage = "O campo DataInicio deve ser uma data válida.")]
    public DateTime DataInicio { get; set; }
    [Required(ErrorMessage = "O campo DataFim é obrigatório.")]
    [DataType(DataType.Date, ErrorMessage = "O campo DataFim deve ser uma data válida.")]
    public DateTime DataFim { get; set; }

    public string StatusReserva { get; set; } = string.Empty; // 0 - Pendente, 1 - Confirmada, 2 - Cancelada
}
