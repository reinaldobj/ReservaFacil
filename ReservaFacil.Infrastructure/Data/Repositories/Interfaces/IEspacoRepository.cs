using System;
using ReservaFacil.Domain.Entities;

namespace ReservaFacil.Infrastructure.Data.Repositories.Interfaces;

public interface IEspacoRepository
{
    IEnumerable<Espaco> Listar();
    Espaco ObterPorId(Guid espacoId);
    Espaco Criar(Espaco espaco);
    bool Atualizar(Guid espacoId, Espaco espaco);
    bool Deletar(Guid espacoId);
    Espaco ObterPorNome(string nome);
}
