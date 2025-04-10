using System;
using ReservaFacil.Application.DTOs;

namespace ReservaFacil.Application.Interfaces;

public interface IEspacoService
{
    IEnumerable<EspacoOutputDto> ListarEspacos();
    EspacoOutputDto ObterEspacoPorId(Guid espacoId);
    EspacoOutputDto CriarEspaco(EspacoInputDto espacoInputDto);
    bool AtualizarEspaco(Guid espacoId, EspacoInputDto espacoInputDto);
    bool DeletarEspaco(Guid espacoId);
}
