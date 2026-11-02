using PetCare.Application.DTOs.AplicacaoVacina;
using PetCare.Domain.Entities;
using PetCare.Application.Interfaces;
using PetCare.Application.Exceptions;
using AutoMapper;

namespace PetCare.Application.Services;

public class AplicacaoVacinaService : IAplicacaoVacinaService
{
    private readonly IAplicacaoVacinaRepository _repository;
    private readonly IMapper _mapper;

    public AplicacaoVacinaService(IAplicacaoVacinaRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<ReadAplicacaoVacinaDto>> GetAllAsync()
    {
        var aplicacoes = await _repository.GetAllAsync();

        return _mapper.Map<IEnumerable<ReadAplicacaoVacinaDto>>(aplicacoes);
    }

    public async Task<ReadAplicacaoVacinaDto?> GetByIdAsync(int id)
    {
        var aplicacao = await _repository.GetByIdAsync(id);

        if (aplicacao == null)
            throw new NotFoundException("Aplicação de vacina não encontrada.");

        return _mapper.Map<ReadAplicacaoVacinaDto>(aplicacao);
    }

    public async Task CreateAsync(CreateAplicacaoVacinaDto dto)
    {
        var aplicacao = _mapper.Map<AplicacaoVacina>(dto);

        await _repository.AddAsync(aplicacao);
    }

    public async Task UpdateAsync(int id, UpdateAplicacaoVacinaDto dto)
    {
        var aplicacao = await _repository.GetByIdAsync(id);

        if (aplicacao == null)
            throw new NotFoundException("Aplicação de vacina não encontrada.");

        _mapper.Map(dto, aplicacao);

        await _repository.UpdateAsync(aplicacao);
    }

    public async Task DeleteAsync(int id)
    {
        await _repository.DeleteAsync(id);
    }
}