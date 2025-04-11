using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ReservaFacil.Application.Interfaces;
using ReservaFacil.Application.DTOs.Usuario;

namespace ReservaFacil.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        private readonly IUsuarioService _usuarioService;
        private readonly ILogger<UsuarioController> _logger;

        public UsuarioController(IUsuarioService usuarioService, ILogger<UsuarioController> logger)
        {
            _logger = logger;
            _usuarioService = usuarioService;
        }

        [HttpGet]
        public IActionResult ListarUsuarios()
        {
            _logger.LogInformation("Listando todos os usuários.");
            var usuarios = _usuarioService.ListarUsuarios();
            return Ok(usuarios);
        }

        [HttpGet("{id}")]
        public IActionResult ObterUsuarioPorId(Guid id)
        {
            _logger.LogInformation($"Buscando usuário com ID: {id}");

            if (id == Guid.Empty)
            {
                _logger.LogWarning("ID inválido fornecido.");
                return BadRequest("ID inválido fornecido.");
            }

            var usuarioOutputDto = _usuarioService.ObterPorId(id);

            if (usuarioOutputDto == null)
            {
                _logger.LogWarning($"Usuário com ID {id} não encontrado.");
                return NotFound($"Usuário com ID {id} não encontrado.");
            }

            _logger.LogInformation($"Usuário encontrado: {usuarioOutputDto}");
            return Ok(usuarioOutputDto);
        }

        [HttpPost]
        public IActionResult CriarUsuario([FromBody] UsuarioInputDto usuarioInputDto)
        {
            _logger.LogInformation("Criando novo usuário.");

            if (usuarioInputDto == null)
            {
                _logger.LogWarning("Dados inválidos fornecidos para criação de usuário.");
                return BadRequest("Dados inválidos fornecidos para criação de usuário.");
            }

            var usuarioOutputDto = _usuarioService.Criar(usuarioInputDto);
            return CreatedAtAction(nameof(ObterUsuarioPorId), new { id = usuarioOutputDto.Id }, usuarioOutputDto);
        }

        [HttpPut("{id}")]
        public IActionResult AtualizarUsuario(Guid id, [FromBody] UsuarioInputDto usuarioInputDto)
        {
            _logger.LogInformation($"Atualizando usuário com ID: {id}");

            if (id == Guid.Empty || usuarioInputDto == null)
            {
                _logger.LogWarning("ID inválido ou dados inválidos fornecidos para atualização de usuário.");
                return BadRequest("ID inválido ou dados inválidos fornecidos para atualização de usuário.");
            }

            var usuarioAtualizado = _usuarioService.Atualizar(id, usuarioInputDto);

            if (!usuarioAtualizado)
            {
                _logger.LogWarning($"Usuário com ID {id} não encontrado.");
                return NotFound($"Usuário com ID {id} não encontrado.");
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult DeletarUsuario(Guid id)
        {
            _logger.LogInformation($"Deletando usuário com ID: {id}");

            if (id == Guid.Empty)
            {
                _logger.LogWarning("ID inválido fornecido para exclusão de usuário.");
                return BadRequest("ID inválido fornecido para exclusão de usuário.");
            }

            var usuarioDeletado = _usuarioService.Deletar(id);

            if (!usuarioDeletado)
            {
                _logger.LogWarning($"Usuário com ID {id} não encontrado.");
                return NotFound($"Usuário com ID {id} não encontrado.");
            }

            return NoContent();
        }
    }
}
