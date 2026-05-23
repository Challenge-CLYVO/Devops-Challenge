using Microsoft.EntityFrameworkCore;
using PetCare.Domain.Entities;
using PetCare.Application.Interfaces;
using PetCare.Infrastructure.Data;

namespace PetCare.Infrastructure.Repositories;

public class ClinicaRepository : IClinicaRepository
{
    private readonly AppDbContext _context;

    public ClinicaRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Clinica>> GetAllAsync()
    {
        return await _context.Clinicas.ToListAsync();
    }

    public async Task<Clinica?> GetByIdAsync(int id)
    {
        return await _context.Clinicas.FindAsync(id);
    }

    public async Task AddAsync(Clinica clinica)
    {
        await _context.Clinicas.AddAsync(clinica);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Clinica clinica)
    {
        _context.Clinicas.Update(clinica);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var clinica = await _context.Clinicas.FindAsync(id);

        if (clinica != null)
        {
            _context.Clinicas.Remove(clinica);
            await _context.SaveChangesAsync();
        }
    }
}