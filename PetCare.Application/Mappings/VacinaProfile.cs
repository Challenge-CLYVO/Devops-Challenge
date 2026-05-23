using AutoMapper;
using PetCare.Application.DTOs.Vacina;
using PetCare.Domain.Entities;

namespace PetCare.Application.Mappings;

public class VacinaProfile : Profile
{
    public VacinaProfile()
    {
        CreateMap<Vacina, ReadVacinaDto>();

        CreateMap<CreateVacinaDto, Vacina>();

        CreateMap<UpdateVacinaDto, Vacina>();
    }
}