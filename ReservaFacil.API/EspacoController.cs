using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ReservaFacil.Application.DTOs;
using ReservaFacil.Application.Interfaces;

namespace ReservaFacil.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class EspacoController : ControllerBase
    {
        private readonly IEspacoService _espacoService;

        public EspacoController(IEspacoService espacoService)
        {
            _espacoService = espacoService;
        }

        [HttpGet]
        public List<EspacoOutputDto> ListEspacos()
        {
            var espacos = _espacoService.ListarEspacos();

            return espacos.ToList();
        }

        [HttpGet("{id}")]
        public IActionResult GetEspacoById(Guid id)
        {
            if (id == Guid.Empty)
            {
                return BadRequest("ID Inválido");
            }

            var espacoOutputDto = _espacoService.ObterEspacoPorId(id);

            return Ok(espacoOutputDto);
        }

        [HttpPost]
        public IActionResult CreateEspaco([FromBody] EspacoInputDto espacoInputDto)
        {
            if (espacoInputDto == null)
            {
                return BadRequest("Dados inválidos");
            }

            var espacoOutputDto = _espacoService.CriarEspaco(espacoInputDto);

            return CreatedAtAction(nameof(GetEspacoById), new { id = espacoOutputDto.Id }, espacoOutputDto);
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteEspaco(Guid id)
        {
            if (id == Guid.Empty)
            {
                return BadRequest("Id Inválido");
            }

            var result = _espacoService.DeletarEspaco(id);

            if (!result)
            {
                return NotFound("Espaço não encontrado ou já foi deletado.");
            }

            return Ok();
        }

        [HttpPut("{id}")]
        public IActionResult UpdateEspaco(Guid id, [FromBody] EspacoInputDto espacoInputDto)
        {
            if (id == Guid.Empty || espacoInputDto == null)
            {
                return BadRequest("Id do espaço inválido ou espaço não pode ser nulo.");
            }

            var result = _espacoService.AtualizarEspaco(id, espacoInputDto);

            if (!result)
            {
                return NotFound("Não foi possível atualizar o espaço.");
            }

            return Ok();
        }
    }
}
