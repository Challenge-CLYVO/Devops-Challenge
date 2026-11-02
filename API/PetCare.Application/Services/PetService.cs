using PetCare.Application.DTOs.Pet;
using PetCare.Application.Interfaces;
using PetCare.Domain.Entities;
using PetCare.Application.Exceptions;
using AutoMapper;
using Microsoft.Extensions.Logging;

namespace PetCare.Application.Services;

public class PetService : IPetService
{
    private readonly IPetRepository _repository;
    private readonly IMapper _mapper;
    private readonly ILogger<PetService> _logger;

    public PetService(
        IPetRepository repository,
        IMapper mapper,
        ILogger<PetService> logger)
    {
        _repository = repository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<IEnumerable<ReadPetDto>> GetAllAsync()
    {
        _logger.LogInformation(
            "Iniciando consulta de todos os pets.");

        var pets = await _repository.GetAllAsync();

        var result = _mapper.Map<IEnumerable<ReadPetDto>>(pets);

        _logger.LogInformation(
            "Consulta de pets concluída. Quantidade: {Quantidade}",
            result.Count());

        return result;
    }

    public async Task<ReadPetDto?> GetByIdAsync(int id)
    {
        _logger.LogInformation(
            "Consultando pet. PetId: {PetId}",
            id);

        var pet = await _repository.GetByIdAsync(id);

        if (pet == null)
            throw new NotFoundException("Pet não encontrado.");

        _logger.LogInformation(
            "Pet encontrado. PetId: {PetId}",
            id);

        return _mapper.Map<ReadPetDto>(pet);
    }

    public async Task CreateAsync(CreatePetDto dto)
    {
        _logger.LogInformation(
            "Iniciando criação de um novo pet.");

        var pet = _mapper.Map<Pet>(dto);

        await _repository.AddAsync(pet);

        _logger.LogInformation(
            "Pet criado com sucesso. PetId: {PetId}",
            pet.IdPet);
    }

    public async Task UpdateAsync(int id, UpdatePetDto dto)
    {
        _logger.LogInformation(
            "Iniciando atualização do pet. PetId: {PetId}",
            id);

        var pet = await _repository.GetByIdAsync(id);

        if (pet == null)
            throw new NotFoundException("Pet não encontrado.");

        _mapper.Map(dto, pet);

        await _repository.UpdateAsync(pet);

        _logger.LogInformation(
            "Pet atualizado com sucesso. PetId: {PetId}",
            id);
    }

    public async Task DeleteAsync(int id)
    {
        _logger.LogInformation(
            "Iniciando exclusão do pet. PetId: {PetId}",
            id);

        await _repository.DeleteAsync(id);

        _logger.LogInformation(
            "Pet excluído com sucesso. PetId: {PetId}",
            id);
    }
}