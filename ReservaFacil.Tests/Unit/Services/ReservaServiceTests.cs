using AutoMapper;
using Moq;
using ReservaFacil.Application.Interfaces;
using ReservaFacil.Application.Services;
using ReservaFacil.Domain.Entities;
using ReservaFacil.Domain.Exceptions;
using ReservaFacil.Infrastructure.Data.Repositories.Interfaces;
using Xunit;

namespace ReservaFacil.Tests.Unit.Services
{
    public class ReservaServiceTests
    {
        [Fact]
        public void Deletar_ShouldThrowNotFound_WhenReservaNaoExiste()
        {
            var repo = new Mock<IReservaRepository>();
            repo.Setup(r => r.ObterPorId(It.IsAny<Guid>())).Returns((Reserva?)null);
            var usuarioService = new Mock<IUsuarioService>();
            var espacoService = new Mock<IEspacoService>();
            var mapper = new Mock<IMapper>();
            var service = new ReservaService(repo.Object, usuarioService.Object, espacoService.Object, mapper.Object);

            Assert.Throws<NotFoundException>(() => service.Deletar(Guid.NewGuid()));
        }
    }
}
