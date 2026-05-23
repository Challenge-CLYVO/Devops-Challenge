using AutoMapper;
using PetCare.Application.DTOs.Pet;
using PetCare.Domain.Entities;

namespace PetCare.Application.Mappings;

public class PetProfile : Profile
{
    public PetProfile()
    {
        CreateMap<Pet, ReadPetDto>();

        CreateMap<CreatePetDto, Pet>();

        CreateMap<UpdatePetDto, Pet>();
    }
}