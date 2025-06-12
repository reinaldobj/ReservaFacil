using System;
using AutoMapper;
using Moq;
using ReservaFacil.Application.DTOs.Espaco;
using ReservaFacil.Application.Mappings;
using ReservaFacil.Application.Services;
using ReservaFacil.Domain.Entities;
using ReservaFacil.Domain.Exceptions;
using ReservaFacil.Infrastructure.Data.Repositories.Interfaces;
using Xunit;

namespace ReservaFacil.UnitTests.Unit.Services
{
    public class EspacoServiceTests
    {
        private readonly IMapper _mapper;

        public EspacoServiceTests()
        {
            var config = new MapperConfiguration(cfg => cfg.AddProfile<EspacoProfile>());
            _mapper = config.CreateMapper();
        }

        [Fact]
        public void Criar_ShouldThrow_WhenNomeExists()
        {
            var repo = new Mock<IEspacoRepository>();
            repo.Setup(r => r.ObterPorNome("Sala"))
                .Returns(new Espaco { Id = Guid.NewGuid(), Nome = "Sala" });
            var service = new EspacoService(_mapper, repo.Object);
            var dto = new EspacoInputDto { Nome = "Sala", Descricao = "", Capacidade = 1, TipoEspaco = "Auditorio" };

            Assert.Throws<BusinessException>(() => service.Criar(dto));
        }
    }
}
