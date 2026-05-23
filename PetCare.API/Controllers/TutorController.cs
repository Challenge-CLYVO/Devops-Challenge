using Microsoft.AspNetCore.Mvc;
using PetCare.Application.DTOs.Tutor;
using PetCare.Application.Interfaces;

namespace PetCare.API.Controllers;

/// <summary>
/// Controller responsável pelo gerenciamento de tutores.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class TutorController : ControllerBase
{
    private readonly ITutorService _service;

    public TutorController(ITutorService service)
    {
        _service = service;
    }

    /// <summary>
    /// Retorna todos os tutores cadastrados.
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var tutores = await _service.GetAllAsync();

        return Ok(tutores);
    }

    /// <summary>
    /// Busca um tutor pelo ID.
    /// </summary>
    /// <param name="id">ID do tutor.</param>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var tutor = await _service.GetByIdAsync(id);

        return Ok(tutor);
    }

    /// <summary>
    /// Cria um novo tutor.
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> Create(CreateTutorDto dto)
    {
        await _service.CreateAsync(dto);

        return Created("", dto);
    }

    /// <summary>
    /// Atualiza um tutor existente.
    /// </summary>
    /// <param name="id">ID do tutor.</param>
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, UpdateTutorDto dto)
    {
        await _service.UpdateAsync(id, dto);

        return NoContent();
    }

    /// <summary>
    /// Remove um tutor.
    /// </summary>
    /// <param name="id">ID do tutor.</param>
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _service.DeleteAsync(id);

        return NoContent();
    }
}