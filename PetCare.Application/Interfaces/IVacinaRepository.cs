using PetCare.Domain.Entities;

namespace PetCare.Application.Interfaces;

public interface IVacinaRepository
{
    Task<IEnumerable<Vacina>> GetAllAsync();

    Task<Vacina?> GetByIdAsync(int id);

    Task AddAsync(Vacina vacina);

    Task UpdateAsync(Vacina vacina);

    Task DeleteAsync(int id);
}