using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using ReservaFacil.API.Controllers;
using ReservaFacil.Application.DTOs.Reserva;
using ReservaFacil.Application.Interfaces;
using Xunit;

namespace ReservaFacil.UnitTests.Unit.Controllers
{
    public class ReservaControllerTests
    {
        [Fact]
        public void ObterReservaPorId_InvalidId_ReturnsBadRequest()
        {
            var serviceMock = new Mock<IReservaService>();
            var logger = new Mock<ILogger<ReservaController>>();
            var controller = new ReservaController(serviceMock.Object, logger.Object);

            var result = controller.ObterReservaPorId(Guid.Empty);

            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public void ListarReservas_ReturnsOkWithData()
        {
            var serviceMock = new Mock<IReservaService>();
            var logger = new Mock<ILogger<ReservaController>>();
            serviceMock.Setup(s => s.Listar()).Returns(new List<ReservaOutputDto>());
            var controller = new ReservaController(serviceMock.Object, logger.Object);

            var result = controller.ListarReservas();

            Assert.IsType<OkObjectResult>(result);
        }
    }
}
