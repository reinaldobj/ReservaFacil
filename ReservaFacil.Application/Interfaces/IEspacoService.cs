using ReservaFacil.Application.DTOs.Espaco;

namespace ReservaFacil.Application.Interfaces;

public interface IEspacoService
{
    IEnumerable<EspacoOutputDto> Listar();
    EspacoOutputDto ObterPorId(Guid espacoId);
    EspacoOutputDto Criar(EspacoInputDto espacoInputDto);
    bool Atualizar(Guid espacoId, EspacoInputDto espacoInputDto);
    bool Deletar(Guid espacoId);
}
