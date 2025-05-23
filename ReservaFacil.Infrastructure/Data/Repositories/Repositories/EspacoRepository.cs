using System;
using ReservaFacil.Domain.Entities;
using ReservaFacil.Infrastructure.Data.Repositories.Interfaces;

namespace ReservaFacil.Infrastructure.Data.Repositories.Repositories;

public class EspacoRepository : IEspacoRepository
{
    private readonly ReservaFacilDbContext _context;

    public EspacoRepository(ReservaFacilDbContext context)
    {
        _context = context;
    }

    public IEnumerable<Espaco> Listar()
    {
        return _context.Espacos
            .ToList();
    }

    public Espaco ObterPorId(Guid espacoId)
    {
        return _context.Espacos.Find(espacoId) ?? throw new InvalidOperationException("Espaço não encontrado.");
    }

    public Espaco Criar(Espaco espaco)
    {
        _context.Espacos.Add(espaco);
        this.SaveChanges();
        
        return espaco;
    }

    public bool Atualizar(Guid espacoId, Espaco espaco)
    {
        var espacoExistente = ObterPorId(espacoId);
        if (espacoExistente == null) return false;

        espacoExistente.Nome = espaco.Nome;
        espacoExistente.Descricao = espaco.Descricao;
        espacoExistente.Capacidade = espaco.Capacidade;
        espacoExistente.TipoEspaco = espaco.TipoEspaco;
        espacoExistente.Disponivel = espaco.Disponivel;
        espacoExistente.Ativo = espaco.Ativo;

        _context.Espacos.Update(espacoExistente);

        return SaveChanges();
    }

    public bool Deletar(Guid espacoId)
    {
        var espaco = ObterPorId(espacoId);
        if (espaco == null) return false;

        espaco.Ativo = false;
        
        return SaveChanges();
    }

    private bool SaveChanges()
    {
        return _context.SaveChanges() > 0;
    }

    public object ObterPorNome(string nome)
    {
        var espaco = _context.Espacos.FirstOrDefault(e => e.Nome == nome);
        if (espaco == null) return null;
        
        return espaco;
    }
}