using System;
using AutoMapper;
using Microsoft.AspNetCore.Diagnostics;
using ReservaFacil.Application.DTOs.Reserva;
using ReservaFacil.Application.Interfaces;
using ReservaFacil.Domain.Entities;
using ReservaFacil.Domain.Enums;
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
        var usuario = _usuarioService.ObterPorEmail(reservaInputDto.Usuario.Email);
        if (usuario == null)
        {
            throw new Exception("Usuário não encontrado.");
        }

        var espaco = _espacoService.ObterEspacoPorId(reservaInputDto.Espaco.Id);
        if (espaco == null)
        {
            throw new Exception("Espaço não encontrado.");
        }

        var reserva = _mapper.Map<Reserva>(reservaInputDto);
        reserva.UsuarioId = usuario.Id;
        reserva.EspacoId = espaco.Id;

        _reservaRepository.CriarReserva(reserva);
        return _mapper.Map<ReservaOutputDto>(reserva);
    }

    public bool Atualizar(Guid id, ReservaInputDto reservaInputDto)
    {
        var reserva = _reservaRepository.ObterReservaPorId(id);
        if (reserva == null)
        {
            throw new Exception("Reserva não encontrada.");
        }

        var usuario = _usuarioService.ObterPorEmail(reservaInputDto.Usuario.Email);
        if (usuario == null)
        {
            throw new Exception("Usuário não encontrado.");
        }

        var espaco = _espacoService.ObterEspacoPorId(reservaInputDto.Espaco.Id);
        if (espaco == null)
        {
            throw new Exception("Espaço não encontrado.");
        }

        reserva.DataInicio = reservaInputDto.DataInicio;
        reserva.DataFim = reservaInputDto.DataFim;
        reserva.StatusReserva = Enum.Parse<StatusReserva>(reservaInputDto.StatusReserva);

        _reservaRepository.AtualizarReserva(id,reserva);
        return true;
    }

    public bool Deletar(Guid id)
    {
        var reserva = _reservaRepository.ObterReservaPorId(id);
        if (reserva == null)
        {
            throw new Exception("Reserva não encontrada.");
        }

        reserva.StatusReserva = StatusReserva.Cancelada;
        _reservaRepository.AtualizarReserva(id, reserva);
        return true;
    }

    public List<ReservaOutputDto> ListarReservas()
    {
        var reservas = _reservaRepository.ListarReservas();
        return _mapper.Map<List<ReservaOutputDto>>(reservas);
    }

    public List<ReservaOutputDto> ListarReservasPorUsuario(Guid usuarioId)
    {
        var reservas = _reservaRepository.ObterReservasPorUsuarioId(usuarioId);
        return _mapper.Map<List<ReservaOutputDto>>(reservas);
    }

    public List<ReservaOutputDto> ListarReservasPorEspaco(Guid espacoId)
    {
        var reservas = _reservaRepository.ObterReservasPorEspacoId(espacoId);
        return _mapper.Map<List<ReservaOutputDto>>(reservas);
    }

    public ReservaOutputDto ObterPorId(Guid id)
    {
        var reserva = _reservaRepository.ObterReservaPorId(id);
        if (reserva == null)
        {
            throw new Exception("Reserva não encontrada.");
        }

        return _mapper.Map<ReservaOutputDto>(reserva);
    }

    public List<ReservaOutputDto> ListarReservasPorUsuario(string email)
    {
        var usuario = _usuarioService.ObterPorEmail(email);
        if (usuario == null)
        {
            throw new Exception("Usuário não encontrado.");
        }

        var reservas = _reservaRepository.ObterReservasPorUsuarioId(usuario.Id);
        return _mapper.Map<List<ReservaOutputDto>>(reservas);
    }
}
