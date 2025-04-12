using System;
using Microsoft.EntityFrameworkCore;
using ReservaFacil.Domain.Entities;
using ReservaFacil.Domain.Enums;
using ReservaFacil.Infrastructure.Data.Repositories.Interfaces;

namespace ReservaFacil.Infrastructure.Data.Repositories.Repositories;

public class ReservaRepository : IReservaRepository
{
    private readonly ReservaFacilDbContext _context;

    public ReservaRepository(ReservaFacilDbContext context)
    {
        _context = context;
    }

    public IEnumerable<Reserva> ListarReservas()
    {
        return _context.Reservas
            .Include(r => r.Espaco)
            .Include(r => r.Usuario)
            .ToList();
    }

    public Reserva ObterReservaPorId(Guid reservaId)
    {
        return _context.Reservas
            .Include(r => r.Espaco)
            .Include(r => r.Usuario)
            .FirstOrDefault(r => r.Id == reservaId);
    }

    public Reserva CriarReserva(Reserva reserva)
    {
        _context.Reservas.Add(reserva);
        _context.SaveChanges();
        
        return reserva;
    }

    public bool AtualizarReserva(Guid reservaId, Reserva reserva)
    {
        var reservaExistente = ObterReservaPorId(reservaId);
        if (reservaExistente == null) return false;

        reservaExistente.DataInicio = reserva.DataInicio;
        reservaExistente.DataFim = reserva.DataFim;
        reservaExistente.StatusReserva = reserva.StatusReserva;
        
        _context.Reservas.Update(reservaExistente);
        
        return _context.SaveChanges() > 0;
    }

    public bool DeletarReserva(Guid reservaId)
    {
        var reserva = ObterReservaPorId(reservaId);
        if (reserva == null) return false;

        reserva.StatusReserva = StatusReserva.Cancelada; // ou outro status que represente a exclusão
        _context.Reservas.Update(reserva);
        
        return _context.SaveChanges() > 0;
    }

    public IEnumerable<Reserva> ObterReservasPorEspacoId(Guid espacoId)
    {
        return _context.Reservas
            .Include(r => r.Espaco)
            .Include(r => r.Usuario)
            .Where(r => r.EspacoId == espacoId)
            .ToList();
    }

    public IEnumerable<Reserva> ObterReservasPorUsuarioId(Guid usuarioId)
    {
        return _context.Reservas
            .Include(r => r.Espaco)
            .Include(r => r.Usuario)
            .Where(r => r.UsuarioId == usuarioId)
            .ToList();
    }

    public IEnumerable<Reserva> ObterReservasPorData(DateTime dataInicial, DateTime dataFinal)
    {
        return _context.Reservas
            .Include(r => r.Espaco)
            .Include(r => r.Usuario)
            .Where(r => r.DataInicio >= dataInicial && r.DataFim <= dataFinal)
            .ToList();
    }

}
