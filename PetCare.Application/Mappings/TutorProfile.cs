using AutoMapper;
using PetCare.Application.DTOs.Tutor;
using PetCare.Domain.Entities;

namespace PetCare.Application.Mappings;

public class TutorProfile : Profile
{
    public TutorProfile()
    {
        CreateMap<Tutor, ReadTutorDto>();

        CreateMap<CreateTutorDto, Tutor>();

        CreateMap<UpdateTutorDto, Tutor>();
    }
}