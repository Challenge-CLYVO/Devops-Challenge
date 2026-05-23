using PetCare.Application.DTOs.Consulta;
using PetCare.Application.Interfaces;
using PetCare.Domain.Entities;
using PetCare.Application.Exceptions;
using AutoMapper;

namespace PetCare.Application.Services;

public class ConsultaService : IConsultaService
{
    private readonly IConsultaRepository _repository;
    private readonly IMapper _mapper;

    public ConsultaService(IConsultaRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<ReadConsultaDto>> GetAllAsync()
    {
        var consultas = await _repository.GetAllAsync();

        return _mapper.Map<IEnumerable<ReadConsultaDto>>(consultas);
    }

    public async Task<ReadConsultaDto?> GetByIdAsync(int id)
    {
        var consulta = await _repository.GetByIdAsync(id);

        if (consulta == null)
            throw new NotFoundException("Consulta não encontrada.");

        return _mapper.Map<ReadConsultaDto>(consulta);
    }

    public async Task CreateAsync(CreateConsultaDto dto)
    {
        var consulta = _mapper.Map<Consulta>(dto);

        await _repository.AddAsync(consulta);
    }

    public async Task UpdateAsync(int id, UpdateConsultaDto dto)
    {
        var consulta = await _repository.GetByIdAsync(id);

        if (consulta == null)
            throw new NotFoundException("Consulta não encontrada.");

        _mapper.Map(dto, consulta);

        await _repository.UpdateAsync(consulta);
    }

    public async Task DeleteAsync(int id)
    {
        await _repository.DeleteAsync(id);
    }
}
