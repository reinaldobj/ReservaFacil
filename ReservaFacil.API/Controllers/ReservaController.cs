using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ReservaFacil.API.Helpers;
using ReservaFacil.Application.DTOs;
using ReservaFacil.Application.DTOs.Reserva;
using ReservaFacil.Application.Interfaces;

namespace ReservaFacil.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReservaController : BaseApiController
    {
        private readonly IReservaService _reservaService;
        public ReservaController(IReservaService reservaService, ILogger<ReservaController> logger) : base(logger)
        {
            _reservaService = reservaService;
        }

        [HttpGet]
        [Authorize(Roles = "Administrador")]
        public IActionResult ListarReservas()
        {
            _logger.LogInformation("Listando todas as reservas.");

            var reservas = _reservaService.Listar();

            return RespostaOk(reservas);
        }

        [HttpGet("{id}")]
        [Authorize]
        public IActionResult ObterReservaPorId(Guid id)
        {
            _logger.LogInformation($"Buscando reserva com ID: {id}");
            if (id == Guid.Empty)
            {
                _logger.LogWarning("ID inválido fornecido.");
                return ErroBadRequest("ID inválido fornecido.");
            }

            var reservaOutputDto = _reservaService.ObterPorId(id);
            if (reservaOutputDto == null)
            {
                _logger.LogWarning($"Reserva com ID: {id} não encontrada.");
                return ErroNotFound($"Reserva com ID: {id} não encontrada.");
            }

            return RespostaOk(reservaOutputDto);
        }

        [HttpPost]
        [Authorize]
        public IActionResult CriarReserva([FromBody] ReservaInputDto reservaInputDto)
        {
            _logger.LogInformation("Criando nova reserva.");
            if (reservaInputDto == null)
            {
                _logger.LogWarning("Dados inválidos fornecidos para criação de reserva.");
                return ErroBadRequest("Dados inválidos fornecidos para criação de reserva.");
            }

            var reservaOutputDto = _reservaService.Criar(reservaInputDto);

            var resposta = ApiResponse<ReservaOutputDto>.Ok(reservaOutputDto, "Reserva criada com sucesso.");

            _logger.LogInformation($"Reserva criada com sucesso: {reservaOutputDto.Id}");

            return CreatedAtAction(nameof(ObterReservaPorId), new { id = reservaOutputDto.Id }, resposta);
        }

        [HttpPut("{id}")]
        [Authorize]
        public IActionResult AtualizarReserva(Guid id, [FromBody] ReservaInputDto reservaInputDto)
        {
            if (!EhUsuarioAutorizado(id))
            {       
                return ErroForbidden("Usuário não autorizado a editar esta reserva.");
            }

            _logger.LogInformation($"Atualizando reserva com ID: {id}");
            if (id == Guid.Empty || reservaInputDto == null)
            {
                _logger.LogWarning("ID inválido ou dados inválidos fornecidos para atualização de reserva.");
                return ErroBadRequest("ID inválido ou dados inválidos fornecidos para atualização de reserva.");
            }

            var reservaAtualizaComSucesso = _reservaService.Atualizar(id, reservaInputDto);
            if (!reservaAtualizaComSucesso)
            {
                _logger.LogWarning($"Falha ao atualizar a reserva com ID: {id}.");
                return ErroNotFound($"Falha ao atualizar a reserva com ID: {id}.");
            }

            var reservaAtualizada = _reservaService.ObterPorId(id);

            return RespostaOk(reservaAtualizada, "Reserva atualizada com sucesso.");
        }

        [HttpDelete("{id}")]
        [Authorize]
        public IActionResult DeletarReserva(Guid id)
        {
            
            if (!EhUsuarioAutorizado(id))
            {                
                return ErroForbidden("Usuário não autorizado a atualizar esta reserva.");
            }            

            _logger.LogInformation($"Deletando reserva com ID: {id}");
            if (id == Guid.Empty)
            {
                _logger.LogWarning("ID inválido fornecido para exclusão de reserva.");
                return ErroBadRequest("ID inválido fornecido para exclusão de reserva.");
            }

            var resultado = _reservaService.Deletar(id);
            if (!resultado)
            {
                _logger.LogWarning($"Falha ao deletar a reserva com ID: {id}.");
                return ErroNotFound($"Falha ao deletar a reserva com ID: {id}.");
            }

            return RespostaOk(resultado, "Reserva excluída com sucesso.");
        }



        [HttpGet("usuario/{usuarioId}")]
        public IActionResult ListarReservasPorUsuario(Guid usuarioId)
        {
            _logger.LogInformation($"Listando reservas para o usuário com ID: {usuarioId}");
            if (usuarioId == Guid.Empty)
            {
                _logger.LogWarning("ID inválido fornecido.");
                return ErroBadRequest("ID inválido fornecido.");
            }

            var reservas = _reservaService.ListarPorUsuario(usuarioId);
            return RespostaOk(reservas);
        }

        [HttpGet("espaco/{espacoId}")]
        [Authorize]
        public IActionResult ListarReservasPorEspaco(Guid espacoId)
        {
            _logger.LogInformation($"Listando reservas para o espaço com ID: {espacoId}");
            if (espacoId == Guid.Empty)
            {
                _logger.LogWarning("ID inválido fornecido.");
                return ErroBadRequest("ID inválido fornecido.");
            }

            var reservas = _reservaService.ListarPorEspaco(espacoId);
            return RespostaOk(reservas);
        }


        private bool EhUsuarioAutorizado(Guid id)
        {
            var reserva = _reservaService.ObterPorId(id);

            if (reserva == null)
            {
                _logger.LogWarning($"Reserva com ID: {id} não encontrada.");
                return false;
            }

            return AutorizacaoHelper.EhUsuarioAutorizado(HttpContext, reserva.UsuarioId);
        }
    }
}
