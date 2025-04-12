using System;
using ReservaFacil.Domain.Entities;

namespace ReservaFacil.Infrastructure.Data.Repositories.Interfaces;

public interface IReservaRepository
{
    public IEnumerable<Reserva> Listar();
    public Reserva ObterPorId(Guid reservaId);
    public Reserva Criar(Reserva reserva);
    public bool Atualizar(Guid reservaId, Reserva reserva);
    public bool Deletar(Guid reservaId);
    public IEnumerable<Reserva> ObterPorEspacoId(Guid espacoId);
    public IEnumerable<Reserva> ObterPorUsuarioId(Guid usuarioId);
    public IEnumerable<Reserva> ObterPorData(DateTime dataInicial, DateTime dataFinal);
    bool VerificarConflito(DateTime dataInicio, DateTime DataFim, Guid espacoId);
}
