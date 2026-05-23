using AutoMapper;
using PetCare.Application.DTOs.Consulta;
using PetCare.Domain.Entities;

namespace PetCare.Application.Mappings;

public class ConsultaProfile : Profile
{
    public ConsultaProfile()
    {
        CreateMap<Consulta, ReadConsultaDto>();

        CreateMap<CreateConsultaDto, Consulta>();

        CreateMap<UpdateConsultaDto, Consulta>();
    }
}