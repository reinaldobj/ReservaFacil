using System;
using System.ComponentModel.DataAnnotations;
using AutoMapper;
using Microsoft.AspNetCore.Diagnostics;
using ReservaFacil.Application.DTOs.Espaco;
using ReservaFacil.Application.DTOs.Reserva;
using ReservaFacil.Application.DTOs.Usuario;
using ReservaFacil.Application.Interfaces;
using ReservaFacil.Domain.Entities;
using ReservaFacil.Domain.Enums;
using ReservaFacil.Domain.Exceptions;
using ReservaFacil.Infrastructure.Data.Repositories.Interfaces;

namespace ReservaFacil.Application.Services;

public class ReservaService : IReservaService
{
    private readonly IReservaRepository _reservaRepository;
    private readonly IUsuarioService _usuarioService;
    private readonly IEspacoService _espacoService;

    private readonly IMapper _mapper;

    public ReservaService(IReservaRepository reservaRepository, IUsuarioService usuarioService, IEspacoService espacoService, IMapper mapper)
    {
        _reservaRepository = reservaRepository;
        _usuarioService = usuarioService;
        _espacoService = espacoService;
        _mapper = mapper;
    }

    public ReservaOutputDto Criar(ReservaInputDto reservaInputDto)
    {
        var usuario = _usuarioService.ObterPorId(reservaInputDto.UsuarioId);
        var espaco = _espacoService.ObterPorId(reservaInputDto.EspacoId);

        ValidarReserva(reservaInputDto, usuario, espaco);

        var reserva = _mapper.Map<Reserva>(reservaInputDto);
        reserva.UsuarioId = usuario.Id;
        reserva.EspacoId = espaco.Id;

        _reservaRepository.Criar(reserva);
        return _mapper.Map<ReservaOutputDto>(reserva);
    }

    private void ValidarReserva(ReservaInputDto reservaInputDto, UsuarioOutputDto usuario, EspacoOutputDto espaco)
    {
        if (usuario == null)
        {
            throw new BusinessException("Usuário não encontrado.");
        }

        if (espaco == null)
        {
            throw new BusinessException("Espaço não encontrado.");
        }

        if (espaco.Disponivel == false)
        {
            throw new BusinessException("Espaço não disponível.");
        }

        if (reservaInputDto.DataInicio < DateTime.Now || reservaInputDto.DataFim < DateTime.Now)
        {
            throw new BusinessException("Data de início ou fim inválida.");
        }

        if (reservaInputDto.DataInicio >= reservaInputDto.DataFim)
        {
            throw new BusinessException("Data de início deve ser menor que a data de fim.");
        }

        if (reservaInputDto.DataInicio < DateTime.Now.AddHours(1))
        {
            throw new BusinessException("Data de início deve ser pelo menos 1 hora a partir de agora.");
        }

        if (reservaInputDto.DataInicio.Hour < 8 || reservaInputDto.DataInicio.Hour > 22)
        {
            throw new BusinessException("Horário de início deve ser entre 08:00 e 22:00.");
        }

        if (reservaInputDto.DataFim.Hour < 8 || reservaInputDto.DataFim.Hour > 22)
        {
            throw new BusinessException("Horário de fim deve ser entre 08:00 e 22:00.");
        }

        if (reservaInputDto.DataInicio.DayOfWeek == DayOfWeek.Saturday || reservaInputDto.DataInicio.DayOfWeek == DayOfWeek.Sunday)
        {
            throw new BusinessException("Reservas não são permitidas aos finais de semana.");
        }

        _reservaRepository.VerificarConflito(reservaInputDto.DataInicio, reservaInputDto.DataFim, espaco.Id);
        if (_reservaRepository.VerificarConflito(reservaInputDto.DataInicio, reservaInputDto.DataFim, espaco.Id))
        {
            throw new BusinessException("Conflito de horário com outra reserva.");
        }
    }

    public bool Atualizar(Guid id, ReservaInputDto reservaInputDto)
    {
        var reserva = _reservaRepository.ObterPorId(id);
        if (reserva == null)
        {
            throw new Exception("Reserva não encontrada.");
        }

        var usuario = _usuarioService.ObterPorId(reservaInputDto.UsuarioId);
        var espaco = _espacoService.ObterPorId(reservaInputDto.EspacoId);

        ValidarReserva(reservaInputDto, usuario, espaco);

        reserva.DataInicio = reservaInputDto.DataInicio;
        reserva.DataFim = reservaInputDto.DataFim;
        reserva.StatusReserva = Enum.Parse<StatusReserva>(reservaInputDto.StatusReserva);

        _reservaRepository.Atualizar(id,reserva);
        return true;
    }

    public bool Deletar(Guid id)
    {
        var reserva = _reservaRepository.ObterPorId(id);
        if (reserva == null)
        {
            throw new Exception("Reserva não encontrada.");
        }

        reserva.StatusReserva = StatusReserva.Cancelada;
        _reservaRepository.Atualizar(id, reserva);
        return true;
    }

    public List<ReservaOutputDto> Listar()
    {
        var reservas = _reservaRepository.Listar();
        return _mapper.Map<List<ReservaOutputDto>>(reservas);
    }

    public List<ReservaOutputDto> ListarPorUsuario(Guid usuarioId)
    {
        var reservas = _reservaRepository.ObterPorUsuarioId(usuarioId);
        return _mapper.Map<List<ReservaOutputDto>>(reservas);
    }

    public List<ReservaOutputDto> ListarPorEspaco(Guid espacoId)
    {
        var reservas = _reservaRepository.ObterPorEspacoId(espacoId);
        return _mapper.Map<List<ReservaOutputDto>>(reservas);
    }

    public ReservaOutputDto ObterPorId(Guid id)
    {
        var reserva = _reservaRepository.ObterPorId(id);
        if (reserva == null)
        {
            throw new Exception("Reserva não encontrada.");
        }

        return _mapper.Map<ReservaOutputDto>(reserva);
    }

    public List<ReservaOutputDto> ListarPorUsuario(string email)
    {
        var usuario = _usuarioService.ObterPorEmail(email);
        if (usuario == null)
        {
            throw new Exception("Usuário não encontrado.");
        }

        var reservas = _reservaRepository.ObterPorUsuarioId(usuario.Id);
        return _mapper.Map<List<ReservaOutputDto>>(reservas);
    }
}
