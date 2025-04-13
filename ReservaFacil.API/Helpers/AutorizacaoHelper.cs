using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace ReservaFacil.API.Helpers;

public static class AutorizacaoHelper
{
    public static bool EhUsuarioAutorizado(HttpContext context, Guid usuarioDonoId)
    {
        var usuarioId = context.User.FindFirstValue(JwtRegisteredClaimNames.Sub) ?? 
                     context.User.FindFirstValue(ClaimTypes.NameIdentifier);
        var usuarioTipo = context.User.FindFirstValue(ClaimTypes.Role);

        if (usuarioId == null || usuarioTipo == null)
        {
            return false;
        }

        if (Guid.TryParse(usuarioId, out Guid id) && id == usuarioDonoId)
        {
            return true;
        }

        return usuarioTipo.Equals("Admin", StringComparison.OrdinalIgnoreCase);
    }
}
