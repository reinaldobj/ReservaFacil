using System;
using AutoMapper;
using ReservaFacil.Application.DTOs.Reserva;
using ReservaFacil.Domain.Entities;
using ReservaFacil.Domain.Enums;

namespace ReservaFacil.Application.Mappings;

public class ReservaProfile : Profile
{
    public ReservaProfile()
    {
        CreateMap<Reserva, ReservaOutputDto>()
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.StatusReserva.ToString()));

        CreateMap<ReservaInputDto, Reserva>()
            .ForMember(dest => dest.StatusReserva, opt => opt.MapFrom(src => Enum.Parse<StatusReserva>(src.StatusReserva)))
            .ForMember(dest => dest.Id, opt => opt.Ignore()); // Ignora o Id, pois ele será gerado automaticamente no banco de dados
    }
}
