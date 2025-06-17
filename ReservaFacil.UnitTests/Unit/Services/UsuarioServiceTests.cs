using System;
using System.ComponentModel.DataAnnotations;
using AutoMapper;
using Moq;
using ReservaFacil.Application.DTOs.Usuario;
using ReservaFacil.Application.Mappings;
using ReservaFacil.Application.Services;
using ReservaFacil.Domain.Entities;
using ReservaFacil.Infrastructure.Data.Repositories.Interfaces;
using Xunit;

namespace ReservaFacil.UnitTests.Unit.Services
{
    public class UsuarioServiceTests
    {
        private readonly IMapper _mapper;

        public UsuarioServiceTests()
        {
            var config = new MapperConfiguration(cfg => cfg.AddProfile<UsuarioProfile>());
            _mapper = config.CreateMapper();
        }

        [Fact]
        public void Criar_ShouldThrow_WhenEmailExists()
        {
            var repo = new Mock<IUsuarioRepository>();
            repo.Setup(r => r.ObterPorEmail("exist@test.com")).Returns(new Usuario { Id = Guid.NewGuid() });
            var service = new UsuarioService(repo.Object, _mapper);
            var dto = new UsuarioInputDto { Nome = "User", Email = "exist@test.com", Senha = "123456", TipoUsuario = "Administrador" };

            Assert.Throws<Domain.Exceptions.ValidationException>(() => service.Criar(dto));
        }
    }
}
