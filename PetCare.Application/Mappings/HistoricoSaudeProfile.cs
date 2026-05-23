using AutoMapper;
using PetCare.Application.DTOs.HistoricoSaude;
using PetCare.Domain.Entities;

namespace PetCare.Application.Mappings;

public class HistoricoSaudeProfile : Profile
{
    public HistoricoSaudeProfile()
    {
        CreateMap<HistoricoSaude, ReadHistoricoSaudeDto>();

        CreateMap<CreateHistoricoSaudeDto, HistoricoSaude>();

        CreateMap<UpdateHistoricoSaudeDto, HistoricoSaude>();
    }
}