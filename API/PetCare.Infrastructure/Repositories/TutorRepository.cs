using Microsoft.EntityFrameworkCore;
using PetCare.Application.Interfaces;
using PetCare.Domain.Entities;
using PetCare.Infrastructure.Data;

namespace PetCare.Infrastructure.Repositories;

public class TutorRepository : ITutorRepository
{
    private readonly AppDbContext _context;

    public TutorRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Tutor>> GetAllAsync()
    {
        return await _context.Tutors.ToListAsync();
    }

    public async Task<Tutor?> GetByIdAsync(int id)
    {
        return await _context.Tutors.FindAsync(id);
    }

    public async Task AddAsync(Tutor tutor)
    {
        _context.Tutors.Add(tutor);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Tutor tutor)
    {
        _context.Tutors.Update(tutor);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var tutor = await _context.Tutors.FindAsync(id);

        if (tutor != null)
        {
            _context.Tutors.Remove(tutor);
            await _context.SaveChangesAsync();
        }
    }
}