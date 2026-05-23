using PetCare.Application.DTOs.Pet;
using PetCare.Application.Interfaces;
using PetCare.Domain.Entities;
using PetCare.Application.Exceptions;
using AutoMapper;

namespace PetCare.Application.Services;

public class PetService : IPetService
{
    private readonly IPetRepository _repository;
    private readonly IMapper _mapper;

    public PetService(IPetRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<ReadPetDto>> GetAllAsync()
    {
        var pets = await _repository.GetAllAsync();

        return _mapper.Map<IEnumerable<ReadPetDto>>(pets);
    }

    public async Task<ReadPetDto?> GetByIdAsync(int id)
    {
        var pet = await _repository.GetByIdAsync(id);

        if (pet == null)
            throw new NotFoundException("Pet não encontrado.");

        return _mapper.Map<ReadPetDto>(pet);
    }

    public async Task CreateAsync(CreatePetDto dto)
    {
        var pet = _mapper.Map<Pet>(dto);

        await _repository.AddAsync(pet);
    }

    public async Task UpdateAsync(int id, UpdatePetDto dto)
    {
        var pet = await _repository.GetByIdAsync(id);

        if (pet == null)
            throw new NotFoundException("Pet não encontrado.");

        _mapper.Map(dto, pet);

        await _repository.UpdateAsync(pet);
    }

    public async Task DeleteAsync(int id)
    {
        await _repository.DeleteAsync(id);
    }
}