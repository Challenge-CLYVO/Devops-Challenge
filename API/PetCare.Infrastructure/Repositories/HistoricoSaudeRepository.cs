using Microsoft.EntityFrameworkCore;
using PetCare.Domain.Entities;
using PetCare.Application.Interfaces;
using PetCare.Infrastructure.Data;

namespace PetCare.Infrastructure.Repositories;

public class HistoricoSaudeRepository : IHistoricoSaudeRepository
{
    private readonly AppDbContext _context;

    public HistoricoSaudeRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<HistoricoSaude>> GetAllAsync()
    {
        return await _context.HistoricosSaude.ToListAsync();
    }

    public async Task<HistoricoSaude?> GetByIdAsync(int id)
    {
        return await _context.HistoricosSaude.FindAsync(id);
    }

    public async Task AddAsync(HistoricoSaude historico)
    {
        await _context.HistoricosSaude.AddAsync(historico);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(HistoricoSaude historico)
    {
        _context.HistoricosSaude.Update(historico);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var historico = await _context.HistoricosSaude.FindAsync(id);

        if (historico != null)
        {
            _context.HistoricosSaude.Remove(historico);
            await _context.SaveChangesAsync();
        }
    }
}