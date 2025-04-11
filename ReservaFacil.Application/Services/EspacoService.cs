using System;
using System.Data.Common;
using AutoMapper;
using ReservaFacil.Application.DTOs;
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

    public bool AtualizarEspaco(Guid espacoId, EspacoInputDto espacoInputDto)
    {
        var espacoExistente = _espacoRepository.ObterEspacoPorId(espacoId);

        if (espacoExistente == null)
            return false;

        Espaco espaco = _mapper.Map<Espaco>(espacoInputDto);
        espaco.Id = espacoId;

        return _espacoRepository.AtualizarEspaco(espacoId, espaco);
    }

    public EspacoOutputDto CriarEspaco(EspacoInputDto espacoInputDto)
    {
        var espacoComMesmoNome = _espacoRepository.ObterEspacoPorNome(espacoInputDto.Nome);

        if (espacoComMesmoNome != null)
            throw new BusinessException("Espaço com o mesmo nome já existe.");


        var espaco = _mapper.Map<Espaco>(espacoInputDto);
        espaco.Id = Guid.NewGuid();
        espaco.Disponivel = true; // Definindo como disponível por padrão

        var espacoOutputDto = _mapper.Map<EspacoOutputDto>(_espacoRepository.CriarEspaco(espaco));

        return espacoOutputDto;

    }

    public bool DeletarEspaco(Guid espacoId)
    {
        return _espacoRepository.DeletarEspaco(espacoId);
    }

    public EspacoOutputDto ObterEspacoPorId(Guid espacoId)
    {
        return _mapper.Map<EspacoOutputDto>(_espacoRepository.ObterEspacoPorId(espacoId));
    }

    public IEnumerable<EspacoOutputDto> ListarEspacos()
    {
        return _mapper.Map<IEnumerable<EspacoOutputDto>>(_espacoRepository.ListarEspacos());
    }
}
