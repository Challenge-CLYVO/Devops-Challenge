using PetCare.Application.DTOs.HistoricoSaude;
using PetCare.Application.Interfaces;
using PetCare.Domain.Entities;
using PetCare.Application.Exceptions;
using AutoMapper;

namespace PetCare.Application.Services;

public class HistoricoSaudeService : IHistoricoSaudeService
{
    private readonly IHistoricoSaudeRepository _repository;
    private readonly IMapper _mapper;

    public HistoricoSaudeService(IHistoricoSaudeRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<ReadHistoricoSaudeDto>> GetAllAsync()
    {
        var historicos = await _repository.GetAllAsync();

        return _mapper.Map<IEnumerable<ReadHistoricoSaudeDto>>(historicos);
    }

    public async Task<ReadHistoricoSaudeDto?> GetByIdAsync(int id)
    {
        var historico = await _repository.GetByIdAsync(id);

        if (historico == null)
            throw new NotFoundException("Histórico de saúde não encontrado.");

        return _mapper.Map<ReadHistoricoSaudeDto>(historico);
    }

    public async Task CreateAsync(CreateHistoricoSaudeDto dto)
    {
        var historico = _mapper.Map<HistoricoSaude>(dto);

        await _repository.AddAsync(historico);
    }

    public async Task UpdateAsync(int id, UpdateHistoricoSaudeDto dto)
    {
        var historico = await _repository.GetByIdAsync(id);

        if (historico == null)
            throw new NotFoundException("Histórico de saúde não encontrado.");

        _mapper.Map(dto, historico);

        await _repository.UpdateAsync(historico);
    }

    public async Task DeleteAsync(int id)
    {
        await _repository.DeleteAsync(id);
    }
}
