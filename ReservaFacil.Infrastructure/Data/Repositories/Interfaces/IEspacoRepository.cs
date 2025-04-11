using System;
using ReservaFacil.Domain.Entities;

namespace ReservaFacil.Infrastructure.Data.Repositories.Interfaces;

public interface IEspacoRepository
{
    IEnumerable<Espaco> ListarEspacos();
    Espaco ObterEspacoPorId(Guid espacoId);
    Espaco CriarEspaco(Espaco espaco);
    bool AtualizarEspaco(Guid espacoId, Espaco espaco);
    bool DeletarEspaco(Guid espacoId);
    object ObterEspacoPorNome(string nome);
}
