using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using ReservaFacil.API.Controllers;
using ReservaFacil.Application.DTOs.Espaco;
using ReservaFacil.Application.Interfaces;
using Xunit;

namespace ReservaFacil.Tests.Unit.Controllers
{
    public class EspacoControllerTests
    {
        [Fact]
        public void Criar_ReturnsCreated_WhenSuccess()
        {
            var serviceMock = new Mock<IEspacoService>();
            var logger = new Mock<ILogger<EspacoController>>();
            var input = new EspacoInputDto { Nome = "Sala", Descricao = "desc", Capacidade = 10, TipoEspaco = "Auditorio" };
            var output = new EspacoOutputDto { Id = Guid.NewGuid(), Nome = "Sala" };
            serviceMock.Setup(s => s.Criar(input)).Returns(output);
            var controller = new EspacoController(serviceMock.Object, logger.Object);

            var result = controller.Criar(input);

            Assert.IsType<CreatedAtActionResult>(result);
        }
    }
}
