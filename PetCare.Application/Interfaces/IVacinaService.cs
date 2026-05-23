using PetCare.Application.DTOs.Vacina;

namespace PetCare.Application.Interfaces;

public interface IVacinaService
{
    Task<IEnumerable<ReadVacinaDto>> GetAllAsync();

    Task<ReadVacinaDto?> GetByIdAsync(int id);

    Task CreateAsync(CreateVacinaDto dto);

    Task UpdateAsync(int id, UpdateVacinaDto dto);

    Task DeleteAsync(int id);
}