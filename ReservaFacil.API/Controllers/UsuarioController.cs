using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ReservaFacil.Application.Interfaces;
using ReservaFacil.Application.DTOs.Usuario;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using ReservaFacil.API.Helpers;
using ReservaFacil.Application.DTOs;
using System.Net;
using AutoMapper;

namespace ReservaFacil.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : BaseApiController
    {
        private readonly IUsuarioService _usuarioService;

        public UsuarioController(IUsuarioService usuarioService, ILogger<UsuarioController> logger) : base(logger)
        {
            _usuarioService = usuarioService;
        }

        [HttpGet]
        [Authorize(Roles = "Administrador")]
        public IActionResult ListarUsuarios()
        {
            _logger.LogInformation("Listando todos os usuários.");

            var usuarios = _usuarioService.ListarUsuarios();
            
            return RespostaOk(usuarios);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Administrador")]
        public IActionResult ObterUsuarioPorId(Guid id)
        {
            _logger.LogInformation($"Buscando usuário com ID: {id}");

            if (id == Guid.Empty)
            {
                _logger.LogWarning("ID inválido fornecido.");

                return ErroBadRequest("ID inválido fornecido.");
            }

            var usuarioOutputDto = _usuarioService.ObterPorId(id);

            if (usuarioOutputDto == null)
            {
                _logger.LogWarning($"Usuário com ID {id} não encontrado.");

                return ErroNotFound($"Usuário com ID {id} não encontrado.");
            }

            _logger.LogInformation($"Usuário encontrado: {usuarioOutputDto}");
            return RespostaOk(usuarioOutputDto);
        }

        [HttpPost]
        public IActionResult CriarUsuario([FromBody] UsuarioInputDto usuarioInputDto)
        {
            _logger.LogInformation("Criando novo usuário.");

            var usuarioOutputDto = _usuarioService.Criar(usuarioInputDto);

            var resposta = ApiResponse<UsuarioOutputDto>.Ok(usuarioOutputDto, "Usuário criado com sucesso.");

            _logger.LogInformation($"Usuário criado com sucesso: {usuarioOutputDto.Id}");

            return CreatedAtAction(nameof(ObterUsuarioPorId), new { id = usuarioOutputDto.Id }, resposta);
        }

        [HttpPut("{id}")]
        [Authorize]
        public IActionResult AtualizarUsuario(Guid id, [FromBody] UsuarioInputDto usuarioInputDto)
        {
            if(!AutorizacaoHelper.EhUsuarioAutorizado(HttpContext, id)){
                _logger.LogWarning($"{id} não autorizado a atualizar este usuário.");

                return ErroForbidden("Usuário não autorizado a atualizar este usuário.");
            }
            
            _logger.LogInformation($"Atualizando usuário com ID: {id}");

            if (id == Guid.Empty)
            {
                _logger.LogWarning("ID inválido ou dados inválidos fornecidos para atualização de usuário.");
                
                return ErroBadRequest("ID inválido ou dados inválidos fornecidos para atualização de usuário.");
            }

            var usuarioAtualizadoComSucesso = _usuarioService.Atualizar(id, usuarioInputDto);

            if (!usuarioAtualizadoComSucesso)
            {
                _logger.LogWarning($"Usuário com ID {id} não encontrado.");

                return ErroNotFound($"Usuário com ID {id} não encontrado.");
            }

            var usuarioAtualizado = _usuarioService.ObterPorId(id);

            return RespostaOk(usuarioAtualizado, "Usuário atualizado com sucesso.");
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Administrador")]
        public IActionResult DeletarUsuario(Guid id)
        {
            _logger.LogInformation($"Deletando usuário com ID: {id}");

            if (id == Guid.Empty)
            {
                _logger.LogWarning("ID inválido fornecido para exclusão de usuário.");

                return ErroBadRequest("ID inválido fornecido para exclusão de usuário.");
            }

            var usuarioDeletado = _usuarioService.Deletar(id);

            if (!usuarioDeletado)
            {
                _logger.LogWarning($"Usuário com ID {id} não encontrado.");

                return ErroNotFound($"Usuário com ID {id} não encontrado.");
            }

            return RespostaOk(id, "Usuário excluído com sucesso.");
        }
    }
}
