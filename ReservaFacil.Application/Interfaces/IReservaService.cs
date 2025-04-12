using System;
using ReservaFacil.Application.DTOs.Reserva;

namespace ReservaFacil.Application.Interfaces;

public interface IReservaService
{
    public ReservaOutputDto Criar(ReservaInputDto reservaInputDto);
    public bool Atualizar(Guid id, ReservaInputDto reservaInputDto);
    public bool Deletar(Guid id);
    public List<ReservaOutputDto> ListarReservas();
    public List<ReservaOutputDto> ListarReservasPorUsuario(Guid usuarioId);
    public List<ReservaOutputDto> ListarReservasPorEspaco(Guid espacoId);
    public ReservaOutputDto ObterPorId(Guid id);
}
