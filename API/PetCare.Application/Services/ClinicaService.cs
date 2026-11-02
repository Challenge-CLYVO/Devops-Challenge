using PetCare.Application.DTOs.Clinica;
using PetCare.Application.Interfaces;
using PetCare.Domain.Entities;
using PetCare.Application.Exceptions;
using AutoMapper;

namespace PetCare.Application.Services;

public class ClinicaService : IClinicaService
{
    private readonly IClinicaRepository _repository;
    private readonly IMapper _mapper;

    public ClinicaService(IClinicaRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<ReadClinicaDto>> GetAllAsync()
    {
        var clinicas = await _repository.GetAllAsync();

        return _mapper.Map<IEnumerable<ReadClinicaDto>>(clinicas);
    }

    public async Task<ReadClinicaDto?> GetByIdAsync(int id)
    {
        var clinica = await _repository.GetByIdAsync(id);

        if (clinica == null)
            throw new NotFoundException("Clínica não encontrada.");

        return _mapper.Map<ReadClinicaDto>(clinica);
    }

    public async Task CreateAsync(CreateClinicaDto dto)
    {
        var clinica = _mapper.Map<Clinica>(dto);

        await _repository.AddAsync(clinica);
    }

    public async Task UpdateAsync(int id, UpdateClinicaDto dto)
    {
        var clinica = await _repository.GetByIdAsync(id);

        if (clinica == null)
            throw new NotFoundException("Clínica não encontrada.");

        _mapper.Map(dto, clinica);

        await _repository.UpdateAsync(clinica);
    }

    public async Task DeleteAsync(int id)
    {
        await _repository.DeleteAsync(id);
    }
}