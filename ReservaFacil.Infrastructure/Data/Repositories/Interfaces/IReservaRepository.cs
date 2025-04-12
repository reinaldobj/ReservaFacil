using System;
using ReservaFacil.Domain.Entities;

namespace ReservaFacil.Infrastructure.Data.Repositories.Interfaces;

public interface IReservaRepository
{
    public IEnumerable<Reserva> ListarReservas();
    public Reserva ObterReservaPorId(Guid reservaId);
    public Reserva CriarReserva(Reserva reserva);
    public bool AtualizarReserva(Guid reservaId, Reserva reserva);
    public bool DeletarReserva(Guid reservaId);
    public IEnumerable<Reserva> ObterReservasPorEspacoId(Guid espacoId);
    public IEnumerable<Reserva> ObterReservasPorUsuarioId(Guid usuarioId);
    public IEnumerable<Reserva> ObterReservasPorData(DateTime dataInicial, DateTime dataFinal);
}
