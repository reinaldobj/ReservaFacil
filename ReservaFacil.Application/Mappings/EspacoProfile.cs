using AutoMapper;
using ReservaFacil.Application.DTOs;
using ReservaFacil.Domain.Entities;
using ReservaFacil.Domain.Enums;

namespace ReservaFacil.Application.Mappings;

public class EspacoProfile : Profile
{
    public EspacoProfile()
    {
        CreateMap<Espaco, EspacoOutputDto>()
            .ForMember(dest => dest.TipoEspaco, opt => opt.MapFrom(src => src.TipoEspaco.ToString()));

        CreateMap<EspacoInputDto, Espaco>()
        .ForMember(dest => dest.TipoEspaco, opt => opt.MapFrom(src => Enum.Parse<TipoEspaco>(src.TipoEspaco)));
    }
}
