using System;
using Microsoft.EntityFrameworkCore;
using ReservaFacil.Domain.Entities;
using ReservaFacil.Infrastructure.Data.Repositories.Interfaces;

namespace ReservaFacil.Infrastructure.Data.Repositories.Repositories;

public class UsuarioRepository : IUsuarioRepository
{
    private readonly ReservaFacilDbContext _context;

    public UsuarioRepository(ReservaFacilDbContext context)
    {
        _context = context;
    }

    public Usuario ObterPorEmail(string email)
    {
        return _context
            .Usuarios
            .FirstOrDefault(u => u.Email == email);
    }

    public Usuario ObterPorId(Guid id)
    {
        return _context
            .Usuarios
            .FirstOrDefault(u => u.Id == id);
    }

    public Usuario ObterPorNome(string nome)
    {
        return _context
            .Usuarios
            .FirstOrDefault(u => u.Nome == nome);
    }

    public Usuario Criar(Usuario usuario)
    {
        _context.Usuarios.Add(usuario);
        SaveChanges();
        
        return usuario;
    }

    public bool Atualizar(Usuario usuario)
    {
        _context.Usuarios.Update(usuario);
        return SaveChanges();
    }

    public bool Deletar(Guid id)
    {
        var usuario = ObterPorId(id);
        if (usuario == null) return false;

        _context.Usuarios.Remove(usuario);
        return SaveChanges();
    }

    public List<Usuario> ListarUsuarios()
    {
        return _context.Usuarios.ToList();
    }

    private bool SaveChanges()
    {
        try
        {
            return _context.SaveChanges() > 0;
        }
        catch (DbUpdateConcurrencyException)
        {
            return false;
        }
    }
}
