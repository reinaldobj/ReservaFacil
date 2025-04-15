using System;
using ReservaFacil.Application.DTOs.Login;

namespace ReservaFacil.Application.Interfaces;

public interface IAuthService
{
    LoginOutputDto? Login(LoginInputDto loginInputDto);
}
