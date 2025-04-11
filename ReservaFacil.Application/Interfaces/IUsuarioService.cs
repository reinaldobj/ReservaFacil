using System;
using ReservaFacil.Application.DTOs.Usuario;

namespace ReservaFacil.Application.Interfaces;

public interface IUsuarioService
{
    public UsuarioOutputDto ObterPorEmail(string email);
    public UsuarioOutputDto ObterPorId(Guid id);
    public UsuarioOutputDto ObterPorNome(string nome);
    public UsuarioOutputDto Criar(UsuarioInputDto usuarioInputDto);
    public bool Atualizar(Guid id, UsuarioInputDto usuarioInputDto);
    public bool Deletar(Guid id);
    public List<UsuarioOutputDto> ListarUsuarios();
}
