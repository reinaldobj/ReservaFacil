using AutoMapper;
using ReservaFacil.Application.DTOs;
using ReservaFacil.Application.DTOs.Espaco;
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
        .ForMember(dest => dest.TipoEspaco, opt => opt.MapFrom(src => Enum.Parse<TipoEspaco>(src.TipoEspaco)))
        .ForMember(dest => dest.Id, opt => opt.Ignore());// Ignora o Id, pois ele será gerado automaticamente no banco de dados
    }
}
