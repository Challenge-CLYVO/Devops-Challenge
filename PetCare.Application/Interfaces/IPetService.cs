using PetCare.Application.DTOs.Pet;

namespace PetCare.Application.Interfaces;

public interface IPetService
{
    Task<IEnumerable<ReadPetDto>> GetAllAsync();

    Task<ReadPetDto?> GetByIdAsync(int id);

    Task CreateAsync(CreatePetDto dto);

    Task UpdateAsync(int id, UpdatePetDto dto);

    Task DeleteAsync(int id);
}