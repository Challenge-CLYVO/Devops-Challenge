using Microsoft.AspNetCore.Mvc;
using PetCare.Application.DTOs.Vacina;
using PetCare.Application.Interfaces;

namespace PetCare.API.Controllers;

/// <summary>
/// Controller responsável pelo gerenciamento de vacinas.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class VacinaController : ControllerBase
{
    private readonly IVacinaService _service;

    public VacinaController(IVacinaService service)
    {
        _service = service;
    }

    /// <summary>
    /// Retorna todas as vacinas cadastradas.
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var vacinas = await _service.GetAllAsync();

        return Ok(vacinas);
    }

    /// <summary>
    /// Busca uma vacina pelo ID.
    /// </summary>
    /// <param name="id">ID da vacina.</param>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var vacina = await _service.GetByIdAsync(id);

        return Ok(vacina);
    }

    /// <summary>
    /// Cria uma nova vacina.
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> Create(CreateVacinaDto dto)
    {
        await _service.CreateAsync(dto);

        return Created("", dto);
    }

    /// <summary>
    /// Atualiza uma vacina existente.
    /// </summary>
    /// <param name="id">ID da vacina.</param>
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, UpdateVacinaDto dto)
    {
        await _service.UpdateAsync(id, dto);

        return NoContent();
    }

    /// <summary>
    /// Remove uma vacina.
    /// </summary>
    /// <param name="id">ID da vacina.</param>
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _service.DeleteAsync(id);

        return NoContent();
    }
}