using System;
using System.Net;
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

    protected IActionResult RespostaOk<T>(T dados, string mensagem = "") => Ok(ApiResponse<T>.Ok(dados, mensagem, (int)HttpStatusCode.OK));
    protected IActionResult ErroBadRequest(string msg) => BadRequest(ApiResponse<string>.Erro(msg, (int)HttpStatusCode.BadRequest));
    protected IActionResult ErroNotFound(string msg) => NotFound(ApiResponse<string>.Erro(msg, (int)HttpStatusCode.NotFound));
    protected IActionResult ErroForbidden(string msg) => StatusCode((int)HttpStatusCode.Forbidden, ApiResponse<string>.Erro(msg, (int)HttpStatusCode.Forbidden));
    protected IActionResult ErroUnauthorized(string msg) => Unauthorized(ApiResponse<string>.Erro(msg, (int)HttpStatusCode.Unauthorized));
}
