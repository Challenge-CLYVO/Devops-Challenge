using Microsoft.AspNetCore.Mvc;
using PetCare.Application.DTOs.Clinica;
using PetCare.Application.Interfaces;

namespace PetCare.API.Controllers;

/// <summary>
/// Controller responsável pelo gerenciamento de clínicas.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class ClinicaController : ControllerBase
{
    private readonly IClinicaService _service;

    public ClinicaController(IClinicaService service)
    {
        _service = service;
    }

    /// <summary>
    /// Retorna todas as clínicas cadastradas.
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var clinicas = await _service.GetAllAsync();

        return Ok(clinicas);
    }

    /// <summary>
    /// Busca uma clínica pelo ID.
    /// </summary>
    /// <param name="id">ID da clínica.</param>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var clinica = await _service.GetByIdAsync(id);

        return Ok(clinica);
    }

    /// <summary>
    /// Cria uma nova clínica.
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> Create(CreateClinicaDto dto)
    {
        await _service.CreateAsync(dto);

        return Created("", dto);
    }

    /// <summary>
    /// Atualiza uma clínica existente.
    /// </summary>
    /// <param name="id">ID da clínica.</param>
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, UpdateClinicaDto dto)
    {
        await _service.UpdateAsync(id, dto);

        return NoContent();
    }

    /// <summary>
    /// Remove uma clínica.
    /// </summary>
    /// <param name="id">ID da clínica.</param>
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _service.DeleteAsync(id);

        return NoContent();
    }
}