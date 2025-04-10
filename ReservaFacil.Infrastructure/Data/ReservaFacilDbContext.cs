using System;
using Microsoft.EntityFrameworkCore;
using ReservaFacil.Domain.Entities;

namespace ReservaFacil.Infrastructure.Data;

public class ReservaFacilDbContext : DbContext 
{
    public ReservaFacilDbContext(DbContextOptions<ReservaFacilDbContext> options) : base(options)
    {
    }

    public DbSet<Reserva> Reservas { get; set; }

    public DbSet<Usuario> Usuarios { get; set; }

    public DbSet<Espaco> Espacos {get; set;}
}
