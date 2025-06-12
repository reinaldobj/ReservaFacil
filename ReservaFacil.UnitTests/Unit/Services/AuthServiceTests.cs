using AutoMapper;
using Microsoft.Extensions.Configuration;
using Moq;
using ReservaFacil.Application.DTOs.Login;
using ReservaFacil.Application.Interfaces;
using ReservaFacil.Application.Services;
using ReservaFacil.Domain.Entities;
using ReservaFacil.Infrastructure.Data.Repositories.Interfaces;
using Xunit;

namespace ReservaFacil.UnitTests.Unit.Services
{
    public class AuthServiceTests
    {
        [Fact]
        public void Login_ReturnsNull_WhenUserNotFound()
        {
            var repo = new Mock<IUsuarioRepository>();
            var mapper = new Mock<IMapper>();
            var tokenService = new Mock<ITokenService>();
            var config = new Mock<IConfiguration>();
            repo.Setup(r => r.ObterPorEmail("none@test.com")).Returns((Usuario?)null);
            var service = new AuthService(repo.Object, config.Object, mapper.Object, tokenService.Object);

            var result = service.Login(new LoginInputDto { Email = "none@test.com", Senha = "123" });

            Assert.Null(result);
        }
    }
}
