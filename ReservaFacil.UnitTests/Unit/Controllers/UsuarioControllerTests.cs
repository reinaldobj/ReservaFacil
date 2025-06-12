using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using ReservaFacil.API.Controllers;
using ReservaFacil.Application.DTOs.Usuario;
using ReservaFacil.Application.Interfaces;
using Xunit;

namespace ReservaFacil.UnitTests.Unit.Controllers
{
    public class UsuarioControllerTests
    {
        [Fact]
        public void CriarUsuario_ReturnsCreated_WhenSuccess()
        {
            var serviceMock = new Mock<IUsuarioService>();
            var logger = new Mock<ILogger<UsuarioController>>();
            var input = new UsuarioInputDto { Nome = "User", Email = "u@test.com", Senha = "123456", TipoUsuario = "Administrador" };
            var output = new UsuarioOutputDto { Id = Guid.NewGuid(), Nome = "User" };
            serviceMock.Setup(s => s.Criar(input)).Returns(output);
            var controller = new UsuarioController(serviceMock.Object, logger.Object);

            var result = controller.CriarUsuario(input);

            Assert.IsType<CreatedAtActionResult>(result);
        }
    }
}
