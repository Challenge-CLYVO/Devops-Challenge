using PetCare.Application.DTOs.Tutor;
using PetCare.Application.Interfaces;
using PetCare.Domain.Entities;
using PetCare.Application.Exceptions;
using AutoMapper;

namespace PetCare.Application.Services;

public class TutorService : ITutorService
{
    private readonly ITutorRepository _repository;
    private readonly IMapper _mapper;

    public TutorService(ITutorRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<ReadTutorDto>> GetAllAsync()
    {
        var tutores = await _repository.GetAllAsync();

        return _mapper.Map<IEnumerable<ReadTutorDto>>(tutores);
    }

    public async Task<ReadTutorDto?> GetByIdAsync(int id)
    {
        var tutor = await _repository.GetByIdAsync(id);

        if (tutor == null)
            throw new NotFoundException("Tutor não encontrado.");

        return _mapper.Map<ReadTutorDto>(tutor);
    }

    public async Task CreateAsync(CreateTutorDto dto)
    {
        var tutor = _mapper.Map<Tutor>(dto);

        await _repository.AddAsync(tutor);
    }

    public async Task UpdateAsync(int id, UpdateTutorDto dto)
    {
        var tutor = await _repository.GetByIdAsync(id);

        if (tutor == null)
            throw new NotFoundException("Tutor não encontrado.");

        _mapper.Map(dto, tutor);

        await _repository.UpdateAsync(tutor);
    }

    public async Task DeleteAsync(int id)
    {
        await _repository.DeleteAsync(id);
    }
}