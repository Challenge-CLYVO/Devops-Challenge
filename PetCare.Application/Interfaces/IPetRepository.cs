using PetCare.Domain.Entities;

namespace PetCare.Application.Interfaces;

public interface IPetRepository
{
    Task<IEnumerable<Pet>> GetAllAsync();

    Task<Pet?> GetByIdAsync(int id);

    Task AddAsync(Pet pet);

    Task UpdateAsync(Pet pet);

    Task DeleteAsync(int id);
}