using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using ReservaFacil.API.Controllers;
using ReservaFacil.Application.DTOs;
using ReservaFacil.Application.DTOs.Login;
using ReservaFacil.Application.Interfaces;
using Xunit;

namespace ReservaFacil.UnitTests.Unit.Controllers
{
    public class AuthControllerTests
    {
        [Fact]
        public void Login_ReturnsOk_WhenCredentialsValid()
        {
            // Arrange
            var serviceMock = new Mock<IAuthService>();
            var loggerMock = new Mock<ILogger<AuthController>>();
            var input = new LoginInputDto { Email = "test@test.com", Senha = "123" };
            var output = new LoginOutputDto { Id = Guid.NewGuid(), Token = "token" };
            serviceMock.Setup(s => s.Login(input)).Returns(output);
            var controller = new AuthController(serviceMock.Object, loggerMock.Object);

            // Act
            var result = controller.Login(input);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<ApiResponse<LoginOutputDto>>(okResult.Value);
            Assert.Equal(output.Token, response.Dados.Token);
        }

        [Fact]
        public void Login_ReturnsUnauthorized_WhenServiceReturnsNull()
        {
            var serviceMock = new Mock<IAuthService>();
            var loggerMock = new Mock<ILogger<AuthController>>();
            var input = new LoginInputDto { Email = "test@test.com", Senha = "123" };
            serviceMock.Setup(s => s.Login(input)).Returns((LoginOutputDto?)null);
            var controller = new AuthController(serviceMock.Object, loggerMock.Object);

            var result = controller.Login(input);

            Assert.IsType<UnauthorizedObjectResult>(result);
        }
    }
}
