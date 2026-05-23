using Microsoft.EntityFrameworkCore;
using PetCare.Domain.Entities;
using PetCare.Application.Interfaces;
using PetCare.Infrastructure.Data;

namespace PetCare.Infrastructure.Repositories;

public class ConsultaRepository : IConsultaRepository
{
    private readonly AppDbContext _context;

    public ConsultaRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Consulta>> GetAllAsync()
    {
        return await _context.Consultas.ToListAsync();
    }

    public async Task<Consulta?> GetByIdAsync(int id)
    {
        return await _context.Consultas.FindAsync(id);
    }

    public async Task AddAsync(Consulta consulta)
    {
        await _context.Consultas.AddAsync(consulta);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Consulta consulta)
    {
        _context.Consultas.Update(consulta);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var consulta = await _context.Consultas.FindAsync(id);

        if (consulta != null)
        {
            _context.Consultas.Remove(consulta);
            await _context.SaveChangesAsync();
        }
    }
}