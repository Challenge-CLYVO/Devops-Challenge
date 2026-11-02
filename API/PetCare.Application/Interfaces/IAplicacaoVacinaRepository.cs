using PetCare.Domain.Entities;

namespace PetCare.Application.Interfaces;

public interface IAplicacaoVacinaRepository
{
    Task<IEnumerable<AplicacaoVacina>> GetAllAsync();

    Task<AplicacaoVacina?> GetByIdAsync(int id);

    Task AddAsync(AplicacaoVacina aplicacaoVacina);

    Task UpdateAsync(AplicacaoVacina aplicacaoVacina);

    Task DeleteAsync(int id);
}