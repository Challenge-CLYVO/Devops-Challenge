using PetCare.Application.DTOs.AplicacaoVacina;

namespace PetCare.Application.Interfaces;

public interface IAplicacaoVacinaService
{
    Task<IEnumerable<ReadAplicacaoVacinaDto>> GetAllAsync();

    Task<ReadAplicacaoVacinaDto?> GetByIdAsync(int id);

    Task CreateAsync(CreateAplicacaoVacinaDto dto);

    Task UpdateAsync(int id, UpdateAplicacaoVacinaDto dto);

    Task DeleteAsync(int id);
}