namespace PetCare.Application.Interfaces;
using PetCare.Domain.Entities;

public interface ITutorRepository
{
    Task<IEnumerable<Tutor>> GetAllAsync();

    Task<Tutor?> GetByIdAsync(int id);

    Task AddAsync(Tutor tutor);

    Task UpdateAsync(Tutor tutor);

    Task DeleteAsync(int id);
}