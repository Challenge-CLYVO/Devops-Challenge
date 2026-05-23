using AutoMapper;
using PetCare.Application.DTOs.AplicacaoVacina;
using PetCare.Domain.Entities;

namespace PetCare.Application.Mappings;

public class AplicacaoVacinaProfile : Profile
{
    public AplicacaoVacinaProfile()
    {
        CreateMap<AplicacaoVacina, ReadAplicacaoVacinaDto>();

        CreateMap<CreateAplicacaoVacinaDto, AplicacaoVacina>();

        CreateMap<UpdateAplicacaoVacinaDto, AplicacaoVacina>();
    }
}