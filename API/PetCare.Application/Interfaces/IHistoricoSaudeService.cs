using PetCare.Application.DTOs.HistoricoSaude;

namespace PetCare.Application.Interfaces;

public interface IHistoricoSaudeService
{
    Task<IEnumerable<ReadHistoricoSaudeDto>> GetAllAsync();

    Task<ReadHistoricoSaudeDto?> GetByIdAsync(int id);

    Task CreateAsync(CreateHistoricoSaudeDto dto);

    Task UpdateAsync(int id, UpdateHistoricoSaudeDto dto);

    Task DeleteAsync(int id);
}