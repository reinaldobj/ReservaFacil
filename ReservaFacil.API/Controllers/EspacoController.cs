using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ReservaFacil.Application.DTOs.Espaco;
using ReservaFacil.Application.Interfaces;
using ReservaFacil.Domain.Exceptions;

namespace ReservaFacil.API.controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class EspacoController : ControllerBase
    {
        private readonly IEspacoService _espacoService;
        private readonly ILogger<EspacoController> _logger;

        public EspacoController(IEspacoService espacoService, ILogger<EspacoController> logger)
        {
            _logger = logger;
            _espacoService = espacoService;
        }

        [HttpGet]
        public List<EspacoOutputDto> ListEspacos()
        {
             _logger.LogInformation("Listando todos os espaços.");
            var espacos = _espacoService.Listar();

            return espacos.ToList();
        }

        [HttpGet("{id}")]
        public IActionResult GetEspacoById(Guid id)
        {
            _logger.LogInformation($"Buscando espaço com ID: {id}");

            if (id == Guid.Empty)
            {
                _logger.LogWarning("ID inválido fornecido.");
                throw new ValidationException("ID inválido fornecido.");
            }

            var espacoOutputDto = _espacoService.ObterPorId(id);

            _logger.LogInformation($"Espaço encontrado: {espacoOutputDto}");

            return Ok(espacoOutputDto);
        }

        [HttpPost]
        [Authorize(Roles = "Administrador")]
        public IActionResult CreateEspaco([FromBody] EspacoInputDto espacoInputDto)
        {
            _logger.LogInformation("Criando novo espaço.");
            if (espacoInputDto == null)
            {
                _logger.LogWarning("Dados inválidos fornecidos para criação de espaço.");
                throw new NotFoundException("Espaço não encontrado.");
            }

            var espacoOutputDto = _espacoService.Criar(espacoInputDto);

            _logger.LogInformation($"Espaço criado com sucesso: {espacoOutputDto.Id}");

            return CreatedAtAction(nameof(GetEspacoById), new { id = espacoOutputDto.Id }, espacoOutputDto);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Administrador")]
        public IActionResult DeleteEspaco(Guid id)
        {
            if (id == Guid.Empty)
            {
                _logger.LogWarning("ID inválido fornecido para exclusão de espaço.");

                throw new ValidationException("ID inválido fornecido para exclusão de espaço.");
            }

            var result = _espacoService.Deletar(id);

            if (!result)
            {
                _logger.LogWarning($"Espaço com ID {id} não encontrado ou já foi deletado.");
                throw new NotFoundException("Espaço não encontrado.");
            }

            _logger.LogInformation($"Espaço com ID {id} deletado com sucesso.");

            return Ok();
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Administrador")]
        public IActionResult UpdateEspaco(Guid id, [FromBody] EspacoInputDto espacoInputDto)
        {
            if (id == Guid.Empty || espacoInputDto == null)
            {
                _logger.LogWarning("ID inválido ou dados inválidos fornecidos para atualização de espaço.");

                throw new ValidationException("Id do espaço inválido ou espaço não pode ser nulo.");
            }

            var result = _espacoService.Atualizar(id, espacoInputDto);

            if (!result)
            {
                _logger.LogWarning($"Não foi possível atualizar o espaço com ID {id}.");

                throw new NotFoundException("Não foi possível atualizar o espaço.");
            }

            _logger.LogInformation($"Espaço com ID {id} atualizado com sucesso.");

            return Ok();
        }
    }
}
