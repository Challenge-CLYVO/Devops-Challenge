namespace PetCare.Application.Interfaces;
using PetCare.Domain.Entities;

public interface IClinicaRepository
{
    Task<IEnumerable<Clinica>> GetAllAsync();
    Task<Clinica?> GetByIdAsync(int id);
    Task AddAsync(Clinica clinica);
    Task UpdateAsync(Clinica clinica);
    Task DeleteAsync(int id);
}