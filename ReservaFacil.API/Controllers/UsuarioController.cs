using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ReservaFacil.Application.Interfaces;
using ReservaFacil.Application.DTOs.Usuario;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

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
        [Authorize(Roles = "Administrador")]
        public IActionResult ListarUsuarios()
        {
            _logger.LogInformation("Listando todos os usuários.");
            var usuarios = _usuarioService.ListarUsuarios();
            return Ok(usuarios);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Administrador")]
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
        [Authorize]
        public IActionResult AtualizarUsuario(Guid id, [FromBody] UsuarioInputDto usuarioInputDto)
        {
            var usuarioLogado = User.FindFirstValue(ClaimTypes.NameIdentifier);
            _logger.LogInformation($"Usuário logado: {usuarioLogado}");

            var TipoUsuario = User.FindFirstValue(ClaimTypes.Role);
            _logger.LogInformation($"Tipo de usuário: {TipoUsuario}");

            if(usuarioLogado != id.ToString() && TipoUsuario != "Administrador")
            {
                _logger.LogWarning($"{usuarioLogado} não autorizado a atualizar este usuário.");
                return Forbid("Usuário não autorizado a atualizar este usuário.");
            }
            
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
        [Authorize(Roles = "Administrador")]
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
