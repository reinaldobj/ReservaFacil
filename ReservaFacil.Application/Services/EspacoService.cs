using AutoMapper;
using ReservaFacil.Application.DTOs.Espaco;
using ReservaFacil.Application.Interfaces;
using ReservaFacil.Domain.Entities;
using ReservaFacil.Domain.Exceptions;
using ReservaFacil.Infrastructure.Data.Repositories.Interfaces;

namespace ReservaFacil.Application.Services;

public class EspacoService : IEspacoService
{
    private readonly IMapper _mapper;

    private readonly IEspacoRepository _espacoRepository;

    public EspacoService(IMapper mapper, IEspacoRepository espacoRepository)
    {
        _mapper = mapper;
        _espacoRepository = espacoRepository;
    }

    public bool Atualizar(Guid espacoId, EspacoInputDto espacoInputDto)
    {
        var espacoExistente = _espacoRepository.ObterPorId(espacoId);

        if (espacoExistente == null)
            return false;

        Espaco espaco = _mapper.Map<Espaco>(espacoInputDto);
        espaco.Id = espacoId;

        return _espacoRepository.Atualizar(espacoId, espaco);
    }

    public EspacoOutputDto Criar(EspacoInputDto espacoInputDto)
    {
        var espacoComMesmoNome = _espacoRepository.ObterPorNome(espacoInputDto.Nome);

        if (espacoComMesmoNome != null)
            throw new BusinessException("Espaço com o mesmo nome já existe.");


        var espaco = _mapper.Map<Espaco>(espacoInputDto);
        espaco.Id = Guid.NewGuid();
        espaco.Disponivel = true; // Definindo como disponível por padrão

        var espacoOutputDto = _mapper.Map<EspacoOutputDto>(_espacoRepository.Criar(espaco));

        return espacoOutputDto;

    }

    public bool Deletar(Guid espacoId)
    {
        return _espacoRepository.Deletar(espacoId);
    }

    public EspacoOutputDto ObterPorId(Guid espacoId)
    {
        return _mapper.Map<EspacoOutputDto>(_espacoRepository.ObterPorId(espacoId));
    }

    public IEnumerable<EspacoOutputDto> Listar()
    {
        return _mapper.Map<IEnumerable<EspacoOutputDto>>(_espacoRepository.Listar());
    }
}
