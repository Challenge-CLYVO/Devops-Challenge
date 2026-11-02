using PetCare.Application.DTOs.Clinica;

namespace PetCare.Application.Interfaces;

public interface IClinicaService
{
    Task<IEnumerable<ReadClinicaDto>> GetAllAsync();
    Task<ReadClinicaDto?> GetByIdAsync(int id);
    Task CreateAsync(CreateClinicaDto dto);
    Task UpdateAsync(int id, UpdateClinicaDto dto);
    Task DeleteAsync(int id);
}