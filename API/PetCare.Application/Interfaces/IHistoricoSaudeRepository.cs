using PetCare.Domain.Entities;

namespace PetCare.Application.Interfaces;

public interface IHistoricoSaudeRepository
{
    Task<IEnumerable<HistoricoSaude>> GetAllAsync();

    Task<HistoricoSaude?> GetByIdAsync(int id);

    Task AddAsync(HistoricoSaude historico);

    Task UpdateAsync(HistoricoSaude historico);

    Task DeleteAsync(int id);
}