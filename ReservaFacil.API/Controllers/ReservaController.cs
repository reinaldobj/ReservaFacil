using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ReservaFacil.Application.DTOs.Reserva;
using ReservaFacil.Application.Interfaces;

namespace ReservaFacil.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReservaController : ControllerBase
    {
        private readonly IReservaService _reservaService;
        private readonly ILogger<ReservaController> _logger;
        public ReservaController(IReservaService reservaService, ILogger<ReservaController> logger)
        {
            _reservaService = reservaService;
            _logger = logger;
        }

        [HttpGet]
        [Authorize(Roles = "Administrador")]
        public IActionResult ListarReservas()
        {
            _logger.LogInformation("Listando todas as reservas.");
            var reservas = _reservaService.ListarReservas();
            return Ok(reservas);
        }

        [HttpGet("{id}")]
        [Authorize]
        public IActionResult ObterReservaPorId(Guid id)
        {
            _logger.LogInformation($"Buscando reserva com ID: {id}");
            if (id == Guid.Empty)
            {
                _logger.LogWarning("ID inválido fornecido.");
                return BadRequest("ID inválido fornecido.");
            }

            var reservaOutputDto = _reservaService.ObterPorId(id);
            if (reservaOutputDto == null)
            {
                _logger.LogWarning($"Reserva com ID: {id} não encontrada.");
                return NotFound($"Reserva com ID: {id} não encontrada.");
            }

            return Ok(reservaOutputDto);
        }

        [HttpPost]
        [Authorize]
        public IActionResult CriarReserva([FromBody] ReservaInputDto reservaInputDto)
        {
            _logger.LogInformation("Criando nova reserva.");
            if (reservaInputDto == null)
            {
                _logger.LogWarning("Dados inválidos fornecidos para criação de reserva.");
                return BadRequest("Dados inválidos fornecidos para criação de reserva.");
            }

            var reservaOutputDto = _reservaService.Criar(reservaInputDto);
            return CreatedAtAction(nameof(ObterReservaPorId), new { id = reservaOutputDto.Id }, reservaOutputDto);
        }

        [HttpPut("{id}")]
        public IActionResult AtualizarReserva(Guid id, [FromBody] ReservaInputDto reservaInputDto)
        {
            if (!ValidarUsuarioLogadoOuAdmin(id))
            {       
                return Forbid("Usuário não autorizado a editar esta reserva.");
            }

            _logger.LogInformation($"Atualizando reserva com ID: {id}");
            if (id == Guid.Empty || reservaInputDto == null)
            {
                _logger.LogWarning("ID inválido ou dados inválidos fornecidos para atualização de reserva.");
                return BadRequest("ID inválido ou dados inválidos fornecidos para atualização de reserva.");
            }

            var resultado = _reservaService.Atualizar(id, reservaInputDto);
            if (!resultado)
            {
                _logger.LogWarning($"Falha ao atualizar a reserva com ID: {id}.");
                return NotFound($"Falha ao atualizar a reserva com ID: {id}.");
            }

            return Ok(resultado);
        }

        [HttpDelete("{id}")]
        public IActionResult DeletarReserva(Guid id)
        {
            
            if (!ValidarUsuarioLogadoOuAdmin(id))
            {                
                return Forbid("Usuário não autorizado a atualizar este usuário.");
            }

            _logger.LogInformation($"Deletando reserva com ID: {id}");
            if (id == Guid.Empty)
            {
                _logger.LogWarning("ID inválido fornecido para exclusão de reserva.");
                return BadRequest("ID inválido fornecido para exclusão de reserva.");
            }

            var resultado = _reservaService.Deletar(id);
            if (!resultado)
            {
                _logger.LogWarning($"Falha ao deletar a reserva com ID: {id}.");
                return NotFound($"Falha ao deletar a reserva com ID: {id}.");
            }

            return Ok(resultado);
        }

        private bool ValidarUsuarioLogadoOuAdmin(Guid id)
        {
            var reserva = _reservaService.ObterPorId(id);

            var usuarioLogado = User.FindFirstValue(ClaimTypes.NameIdentifier);
            _logger.LogInformation($"Usuário logado: {usuarioLogado}");

            var TipoUsuario = User.FindFirstValue(ClaimTypes.Role);
            _logger.LogInformation($"Tipo de usuário: {TipoUsuario}");

            if (usuarioLogado != reserva.UsuarioId.ToString() && TipoUsuario != "Administrador")
            {
                _logger.LogWarning($"Usuário não autorizado a editar esta reserva.");
                return false;
            }

            return true;
        }

        [HttpGet("usuario/{usuarioId}")]
        public IActionResult ListarReservasPorUsuario(Guid usuarioId)
        {
            _logger.LogInformation($"Listando reservas para o usuário com ID: {usuarioId}");
            if (usuarioId == Guid.Empty)
            {
                _logger.LogWarning("ID inválido fornecido.");
                return BadRequest("ID inválido fornecido.");
            }

            var reservas = _reservaService.ListarReservasPorUsuario(usuarioId);
            return Ok(reservas);
        }

        [HttpGet("espaco/{espacoId}")]
        [Authorize]
        public IActionResult ListarReservasPorEspaco(Guid espacoId)
        {
            _logger.LogInformation($"Listando reservas para o espaço com ID: {espacoId}");
            if (espacoId == Guid.Empty)
            {
                _logger.LogWarning("ID inválido fornecido.");
                return BadRequest("ID inválido fornecido.");
            }

            var reservas = _reservaService.ListarReservasPorEspaco(espacoId);
            return Ok(reservas);
        }
    }
}
