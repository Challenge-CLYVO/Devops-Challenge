using PetCare.Application.DTOs.Tutor;

namespace PetCare.Application.Interfaces;

public interface ITutorService
{
    Task<IEnumerable<ReadTutorDto>> GetAllAsync();

    Task<ReadTutorDto?> GetByIdAsync(int id);

    Task CreateAsync(CreateTutorDto dto);

    Task UpdateAsync(int id, UpdateTutorDto dto);

    Task DeleteAsync(int id);
}