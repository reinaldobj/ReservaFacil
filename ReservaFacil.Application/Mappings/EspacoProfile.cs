using System;
using AutoMapper;

namespace ReservaFacil.Application.Mappings;

public class EspacoProfile : Profile
{
    public EspacoProfile()
    {
        CreateMap<Domain.Entities.Espaco, DTOs.EspacoOutputDto>()
            .ForMember(dest => dest.TipoEspaco, opt => opt.MapFrom(src => src.TipoEspaco.ToString()));

        CreateMap<DTOs.EspacoInputDto, Domain.Entities.Espaco>();
    }
}
