using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ReservaFacil.Application.DTOs;
using ReservaFacil.Application.DTOs.Espaco;
using ReservaFacil.Application.Interfaces;
using ReservaFacil.Domain.Exceptions;

namespace ReservaFacil.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EspacoController : BaseApiController
    {
        private readonly IEspacoService _espacoService;

        public EspacoController(IEspacoService espacoService, ILogger<EspacoController> logger) : base(logger)
        {
            _espacoService = espacoService;
        }

        [HttpGet]
        public IActionResult Listar()
        {
             _logger.LogInformation("Listando todos os espaços.");
            var espacos = _espacoService.Listar();

            return RespostaOk(espacos);
        }

        [HttpGet("{id}")]
        public IActionResult ObterPorId(Guid id)
        {
            _logger.LogInformation($"Buscando espaço com ID: {id}");

            if (id == Guid.Empty)
            {
                _logger.LogWarning("ID inválido fornecido.");

                return ErroBadRequest("ID inválido fornecido.");
            }

            var espacoOutputDto = _espacoService.ObterPorId(id);

            if (espacoOutputDto == null)
            {
                _logger.LogWarning($"Espaço com ID {id} não encontrado.");
                return ErroNotFound($"Espaço com ID {id} não encontrado.");
            }

            _logger.LogInformation($"Espaço encontrado: {espacoOutputDto}");

            return RespostaOk(espacoOutputDto);
        }

        [HttpPost]
        [Authorize(Roles = "Administrador")]
        public IActionResult Criar([FromBody] EspacoInputDto espacoInputDto)
        {
            _logger.LogInformation("Criando novo espaço.");

            var espacoOutputDto = _espacoService.Criar(espacoInputDto);

            _logger.LogInformation($"Espaço criado com sucesso: {espacoOutputDto.Id}");

            var resposta = ApiResponse<EspacoOutputDto>.Ok(espacoOutputDto, "Espaço criado com sucesso.");

            return CreatedAtAction(nameof(Criar), new { id = espacoOutputDto.Id }, resposta);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Administrador")]
        public IActionResult Deletar(Guid id)
        {
            if (id == Guid.Empty)
            {
                _logger.LogWarning("ID inválido fornecido para exclusão de espaço.");

                return ErroBadRequest("ID inválido fornecido para exclusão de espaço.");
            }

            var result = _espacoService.Deletar(id);

            if (!result)
            {
                _logger.LogWarning($"Espaço com ID {id} não encontrado ou já foi deletado.");

                return ErroNotFound("Espaço não encontrado.");
            }

            _logger.LogInformation($"Espaço com ID {id} deletado com sucesso.");

            return RespostaOk(id, "Espaço deletado com sucesso.");
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Administrador")]
        public IActionResult Atualizar(Guid id, [FromBody] EspacoInputDto espacoInputDto)
        {
            if (id == Guid.Empty)
            {
                _logger.LogWarning("ID inválido ou dados inválidos fornecidos para atualização de espaço.");

                return ErroBadRequest("ID inválido ou dados inválidos fornecidos para atualização de espaço.");
            }

            var espacoAtualizadoComSucesso = _espacoService.Atualizar(id, espacoInputDto);

            if (!espacoAtualizadoComSucesso)
            {
                _logger.LogWarning($"Não foi possível atualizar o espaço com ID {id}.");

                return ErroNotFound("Não foi possível atualizar o espaço.");
            }

            var espacoAtualizado = _espacoService.ObterPorId(id);

            _logger.LogInformation($"Espaço com ID {id} atualizado com sucesso.");

            return RespostaOk(espacoAtualizado, "Espaço atualizado com sucesso.");
        }
    }
}
