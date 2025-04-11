using System;
using ReservaFacil.Domain.Entities;

namespace ReservaFacil.Infrastructure.Data.Repositories.Interfaces;

public interface IUsuarioRepository
{
    Usuario ObterPorEmail(string email);
    Usuario ObterPorId(Guid id);
    Usuario ObterPorNome(string nome);
    Usuario Criar(Usuario usuario);
    bool Atualizar(Usuario usuario);
    bool Deletar(Guid id);
    List<Usuario> ListarUsuarios();
}
