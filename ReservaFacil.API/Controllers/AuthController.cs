using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ReservaFacil.Application.DTOs;
using ReservaFacil.Application.DTOs.Login;
using ReservaFacil.Application.Interfaces;

namespace ReservaFacil.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : BaseApiController
    {
        private readonly IAuthService _authService;
        public AuthController(IAuthService authService, ILogger<AuthController> logger) : base(logger)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginInputDto loginInputDto)
        {
            _logger.LogInformation("Iniciando processo de login.");

            var loginOutputDto = _authService.Login(loginInputDto);

            if (loginOutputDto == null || string.IsNullOrEmpty(loginOutputDto.Token))
            {
                _logger.LogWarning("Falha no login: credenciais inválidas.");

                return ErroUnauthorized("Falha no login: credenciais inválidas.");
            }

            _logger.LogInformation("Login realizado com sucesso.");

            return RespostaOk(loginOutputDto, "Login realizado com sucesso.");
        }
    }
}
