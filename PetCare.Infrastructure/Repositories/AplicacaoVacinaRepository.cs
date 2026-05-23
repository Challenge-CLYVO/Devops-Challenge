using Microsoft.EntityFrameworkCore;
using PetCare.Domain.Entities;
using PetCare.Application.Interfaces;
using PetCare.Infrastructure.Data;

namespace PetCare.Infrastructure.Repositories;

public class AplicacaoVacinaRepository : IAplicacaoVacinaRepository
{
    private readonly AppDbContext _context;

    public AplicacaoVacinaRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<AplicacaoVacina>> GetAllAsync()
    {
        return await _context.AplicacoesVacina.ToListAsync();
    }

    public async Task<AplicacaoVacina?> GetByIdAsync(int id)
    {
        return await _context.AplicacoesVacina.FindAsync(id);
    }

    public async Task AddAsync(AplicacaoVacina aplicacaoVacina)
    {
        await _context.AplicacoesVacina.AddAsync(aplicacaoVacina);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(AplicacaoVacina aplicacaoVacina)
    {
        _context.AplicacoesVacina.Update(aplicacaoVacina);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var aplicacaoVacina = await GetByIdAsync(id);

        if (aplicacaoVacina != null)
        {
            _context.AplicacoesVacina.Remove(aplicacaoVacina);
            await _context.SaveChangesAsync();
        }
    }
}