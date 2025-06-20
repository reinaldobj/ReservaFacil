using System;
using System.ComponentModel.DataAnnotations;
using AutoMapper;
using ReservaFacil.Application.DTOs.Usuario;
using ReservaFacil.Application.Interfaces;
using ReservaFacil.Domain.Entities;
using ReservaFacil.Infrastructure.Data.Repositories.Interfaces;
using ReservaFacil.Domain.Exceptions;

namespace ReservaFacil.Application.Services;

public class UsuarioService : IUsuarioService
{
    private readonly IUsuarioRepository _usuarioRepository;
    private readonly IMapper _mapper;

    public UsuarioService(IUsuarioRepository usuarioRepository, IMapper mapper)
    {
        _usuarioRepository = usuarioRepository;
        _mapper = mapper;
    }

    public UsuarioOutputDto ObterPorEmail(string email)
    {
        var usuario = _usuarioRepository.ObterPorEmail(email);
        return _mapper.Map<UsuarioOutputDto>(usuario);
    }

    public UsuarioOutputDto ObterPorId(Guid id)
    {
        var usuario = _usuarioRepository.ObterPorId(id);
        return _mapper.Map<UsuarioOutputDto>(usuario);
    }

    public UsuarioOutputDto ObterPorNome(string nome)
    {
        var usuario = _usuarioRepository.ObterPorNome(nome);
        return _mapper.Map<UsuarioOutputDto>(usuario);
    }

    public UsuarioOutputDto Criar(UsuarioInputDto usuarioInputDto)
    {
        var usuarioExistente = _usuarioRepository.ObterPorEmail(usuarioInputDto.Email);
        if (usuarioExistente != null) 
            throw new Domain.Exceptions.ValidationException($"O email {usuarioInputDto.Email} já está em uso.");

        var usuario = _mapper.Map<Usuario>(usuarioInputDto);

        _usuarioRepository.Criar(usuario);

        return _mapper.Map<UsuarioOutputDto>(usuario);
    }

    public bool Atualizar(Guid id, UsuarioInputDto usuarioInputDto)
    {
        var usuario = _usuarioRepository.ObterPorId(id);
        if (usuario == null) 
            throw new Domain.Exceptions.ValidationException($"Usuário com ID {id} não encontrado.");

        _mapper.Map(usuarioInputDto, usuario);
        _usuarioRepository.Atualizar(id, usuario);
        return true;
    }

    public bool Deletar(Guid id)
    {
        var usuario = _usuarioRepository.ObterPorId(id);
        if (usuario == null) 
            throw new Domain.Exceptions.ValidationException($"Usuário com ID {id} não encontrado.");

        _usuarioRepository.Deletar(usuario.Id);
        return true;
    }

    public List<UsuarioOutputDto> ListarUsuarios()
    {
        var usuarios = _usuarioRepository.Listar();
        return _mapper.Map<List<UsuarioOutputDto>>(usuarios);
    }
}
