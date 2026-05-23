using Microsoft.AspNetCore.Mvc;
using PetCare.Application.DTOs.Pet;
using PetCare.Application.Interfaces;

namespace PetCare.API.Controllers;

/// <summary>
/// Controller responsável pelo gerenciamento de pets.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class PetController : ControllerBase
{
    private readonly IPetService _service;

    public PetController(IPetService service)
    {
        _service = service;
    }

    /// <summary>
    /// Retorna todos os pets cadastrados.
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var pets = await _service.GetAllAsync();

        return Ok(pets);
    }

    /// <summary>
    /// Busca um pet pelo ID.
    /// </summary>
    /// <param name="id">ID do pet.</param>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var pet = await _service.GetByIdAsync(id);

        return Ok(pet);
    }

    /// <summary>
    /// Cria um novo pet.
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> Create(CreatePetDto dto)
    {
        await _service.CreateAsync(dto);

        return Created("", dto);
    }

    /// <summary>
    /// Atualiza um pet existente.
    /// </summary>
    /// <param name="id">ID do pet.</param>
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, UpdatePetDto dto)
    {
        await _service.UpdateAsync(id, dto);

        return NoContent();
    }

    /// <summary>
    /// Remove um pet.
    /// </summary>
    /// <param name="id">ID do pet.</param>
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _service.DeleteAsync(id);

        return NoContent();
    }
}