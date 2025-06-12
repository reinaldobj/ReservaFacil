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

    public IEnumerable<Reserva> Listar()
    {
        return _context.Reservas
            .Include(r => r.Espaco)
            .Include(r => r.Usuario)
            .ToList();
    }

    public Reserva ObterPorId(Guid reservaId)
    {
        return _context
            .Reservas
            .AsNoTracking()
            .Include(r => r.Espaco)
            .Include(r => r.Usuario)
            .FirstOrDefault(r => r.Id == reservaId);
    }

    public Reserva Criar(Reserva reserva)
    {
        _context
            .Reservas
            .Add(reserva);

        _context
            .SaveChanges();
        
        return reserva;
    }

    public bool Atualizar(Guid reservaId, Reserva reserva)
    {
        var reservaExistente = _context
            .Reservas
            .FirstOrDefault(r => r.Id == reservaId);

        if (reservaExistente == null) return false;

        reservaExistente.DataInicio = reserva.DataInicio;
        reservaExistente.DataFim = reserva.DataFim;
        reservaExistente.StatusReserva = reserva.StatusReserva;
        
        _context.Reservas.Update(reservaExistente);
        
        return SaveChanges();
    }

    public bool Deletar(Guid reservaId)
    {
        var reserva = _context
            .Reservas
            .FirstOrDefault(r => r.Id == reservaId);
        
        if (reserva == null) return false;

        reserva.StatusReserva = StatusReserva.Cancelada; // ou outro status que represente a exclusão
        _context.Reservas.Update(reserva);
        
        return SaveChanges();
    }

    public IEnumerable<Reserva> ObterPorEspacoId(Guid espacoId)
    {
        return _context
            .Reservas
            .AsNoTracking()
            .Include(r => r.Espaco)
            .Include(r => r.Usuario)
            .Where(r => r.EspacoId == espacoId)
            .ToList();
    }

    public IEnumerable<Reserva> ObterPorUsuarioId(Guid usuarioId)
    {
        return _context
            .Reservas
            .AsNoTracking()
            .Include(r => r.Espaco)
            .Include(r => r.Usuario)
            .Where(r => r.UsuarioId == usuarioId)
            .ToList();
    }

    public IEnumerable<Reserva> ObterPorData(DateTime dataInicial, DateTime dataFinal)
    {
        return _context
            .Reservas
            .AsNoTracking()
            .Include(r => r.Espaco)
            .Include(r => r.Usuario)
            .Where(r => r.DataInicio >= dataInicial && r.DataFim <= dataFinal)
            .ToList();
    }

    private bool SaveChanges()
    {
        try
        {
            return _context
                .SaveChanges() > 0;
        }
        catch (DbUpdateConcurrencyException)
        {
            return false;
        }
    }

    public bool VerificarConflito(DateTime dataInicio, DateTime DataFim, Guid espacoId)
    {
        var reservasConflitantes = _context.Reservas
            .Where(r => r.EspacoId == espacoId && r.StatusReserva != StatusReserva.Cancelada)
            .Any(r => (r.DataInicio < DataFim && r.DataFim > dataInicio));

        return reservasConflitantes;
    }
}
