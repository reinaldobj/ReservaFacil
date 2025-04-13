using System;
using Microsoft.AspNetCore.Mvc;
using ReservaFacil.Application.DTOs;

namespace ReservaFacil.API.Controllers;

public class BaseApiController : ControllerBase
{
    protected readonly ILogger<BaseApiController> _logger;
    public BaseApiController(ILogger<BaseApiController> logger)
    {
        _logger = logger;
    }

    protected IActionResult RespostaOk<T>(T dados, string mensagem = "") => Ok(ApiResponse<T>.Ok(dados, mensagem));
    protected IActionResult ErroBadRequest(string msg) => BadRequest(ApiResponse<string>.Erro(msg));
    protected IActionResult ErroNotFound(string msg) => NotFound(ApiResponse<string>.Erro(msg));
    protected IActionResult ErroForbidden(string msg) => StatusCode(403, ApiResponse<string>.Erro(msg));
    protected IActionResult ErroUnauthorized(string msg) => Unauthorized(ApiResponse<string>.Erro(msg));
}
