using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ReservaFacil.Application.DTOs.Login;
using ReservaFacil.Application.Interfaces;

namespace ReservaFacil.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly ILogger<AuthController> _logger;

        public AuthController(IAuthService authService, ILogger<AuthController> logger)
        {
            _logger = logger;
            _authService = authService;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginInputDto loginInputDto)
        {
            _logger.LogInformation("Iniciando processo de login.");

            if (loginInputDto == null || string.IsNullOrEmpty(loginInputDto.Email) || string.IsNullOrEmpty(loginInputDto.Senha))
            {
                _logger.LogWarning("Dados inválidos fornecidos para login.");
                return BadRequest("Dados inválidos fornecidos para login.");
            }

            var loginOutputDto = _authService.Login(loginInputDto);

            if (loginOutputDto == null || string.IsNullOrEmpty(loginOutputDto.Token))
            {
                _logger.LogWarning("Falha no login: credenciais inválidas.");
                return Unauthorized("Credenciais inválidas.");
            }

            _logger.LogInformation("Login realizado com sucesso.");
            return Ok(loginOutputDto);
        }
    }
}
