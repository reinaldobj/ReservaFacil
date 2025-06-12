using System;
using ReservaFacil.Application.DTOs.Reserva;

namespace ReservaFacil.Application.Interfaces;

public interface IReservaService
{
    public ReservaOutputDto Criar(ReservaInputDto reservaInputDto);
    public bool Atualizar(Guid id, ReservaInputDto reservaInputDto);
    public bool Deletar(Guid id);
    public List<ReservaOutputDto> Listar();
    public List<ReservaOutputDto> ListarPorUsuario(Guid usuarioId);
    public List<ReservaOutputDto> ListarPorUsuario(string email);
    public List<ReservaOutputDto> ListarPorEspaco(Guid espacoId);
    public ReservaOutputDto ObterPorId(Guid id);
}
