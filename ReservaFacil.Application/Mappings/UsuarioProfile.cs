using AutoMapper;
using ReservaFacil.Domain.Entities;
using ReservaFacil.Application.DTOs.Usuario;
using ReservaFacil.Domain.Enums;

namespace ReservaFacil.Application.Mappings;

public class UsuarioProfile : Profile
{
    public UsuarioProfile()
    {
        CreateMap<Usuario, UsuarioOutputDto>()
            .ForMember(dest => dest.TipoUsuario, opt => opt.MapFrom(src => src.TipoUsuario.ToString()));

        CreateMap<UsuarioInputDto, Usuario>()
            .ForMember(dest => dest.TipoUsuario, opt => opt.MapFrom(src => Enum.Parse<TipoUsuario>(src.TipoUsuario)))
            .ForMember(dest => dest.SenhaHash, opt => opt.MapFrom(src => BCrypt.Net.BCrypt.HashPassword(src.Senha)));
    }
}
