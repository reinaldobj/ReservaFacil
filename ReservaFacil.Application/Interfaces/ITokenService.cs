using System;
using ReservaFacil.Domain.Entities;

namespace ReservaFacil.Application.Interfaces;

public interface ITokenService
{
    string GerarToken(Usuario usuario);
}
