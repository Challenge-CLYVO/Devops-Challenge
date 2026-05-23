using PetCare.Application.DTOs.Vacina;
using PetCare.Application.Interfaces;
using PetCare.Domain.Entities;
using PetCare.Application.Exceptions;
using AutoMapper;

namespace PetCare.Application.Services;

public class VacinaService : IVacinaService
{
    private readonly IVacinaRepository _repository;
    private readonly IMapper _mapper;

    public VacinaService(IVacinaRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<ReadVacinaDto>> GetAllAsync()
    {
        var vacinas = await _repository.GetAllAsync();

        return _mapper.Map<IEnumerable<ReadVacinaDto>>(vacinas);
    }

    public async Task<ReadVacinaDto?> GetByIdAsync(int id)
    {
        var vacina = await _repository.GetByIdAsync(id);

        if (vacina == null)
            throw new NotFoundException("Vacina não encontrada.");

        return _mapper.Map<ReadVacinaDto>(vacina);
    }

    public async Task CreateAsync(CreateVacinaDto dto)
    {
        var vacina = _mapper.Map<Vacina>(dto);

        await _repository.AddAsync(vacina);
    }

    public async Task UpdateAsync(int id, UpdateVacinaDto dto)
    {
        var vacina = await _repository.GetByIdAsync(id);

        if (vacina == null)
            throw new NotFoundException("Vacina não encontrada.");

        _mapper.Map(dto, vacina);

        await _repository.UpdateAsync(vacina);
    }

    public async Task DeleteAsync(int id)
    {
        await _repository.DeleteAsync(id);
    }
}