using PetCare.Application.DTOs.Consulta;

namespace PetCare.Application.Interfaces;

public interface IConsultaService
{
    Task<IEnumerable<ReadConsultaDto>> GetAllAsync();

    Task<ReadConsultaDto?> GetByIdAsync(int id);

    Task CreateAsync(CreateConsultaDto dto);

    Task UpdateAsync(int id, UpdateConsultaDto dto);

    Task DeleteAsync(int id);
}