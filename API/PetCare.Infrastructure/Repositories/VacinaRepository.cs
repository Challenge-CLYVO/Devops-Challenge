using Microsoft.EntityFrameworkCore;
using PetCare.Application.Interfaces;
using PetCare.Domain.Entities;
using PetCare.Infrastructure.Data;

namespace PetCare.Infrastructure.Repositories;

public class VacinaRepository : IVacinaRepository
{
    private readonly AppDbContext _context;

    public VacinaRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Vacina>> GetAllAsync()
    {
        return await _context.Vacinas.ToListAsync();
    }

    public async Task<Vacina?> GetByIdAsync(int id)
    {
        return await _context.Vacinas.FindAsync(id);
    }

    public async Task AddAsync(Vacina vacina)
    {
        _context.Vacinas.Add(vacina);

        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Vacina vacina)
    {
        _context.Vacinas.Update(vacina);

        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var vacina = await _context.Vacinas.FindAsync(id);

        if (vacina != null)
        {
            _context.Vacinas.Remove(vacina);

            await _context.SaveChangesAsync();
        }
    }
}