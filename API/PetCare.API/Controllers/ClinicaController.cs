using Microsoft.AspNetCore.Mvc;
using PetCare.Application.DTOs.Clinica;
using PetCare.Application.Interfaces;

namespace PetCare.API.Controllers;

/// <summary>
/// Controller responsÃ¡vel pelo gerenciamento de clÃ­nicas.
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
    /// Retorna todas as clÃ­nicas cadastradas.
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var clinicas = await _service.GetAllAsync();

        return Ok(clinicas);
    }

    /// <summary>
    /// Busca uma clÃ­nica pelo ID.
    /// </summary>
    /// <param name="id">ID da clÃ­nica.</param>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var clinica = await _service.GetByIdAsync(id);

        return Ok(clinica);
    }

    /// <summary>
    /// Cria uma nova clÃ­nica.
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> Create(CreateClinicaDto dto)
    {
        await _service.CreateAsync(dto);

        return Created("", dto);
    }

    /// <summary>
    /// Atualiza uma clÃ­nica existente.
    /// </summary>
    /// <param name="id">ID da clÃ­nica.</param>
    /// <param name="dto">Dados atualizados da clínica.</param>
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, UpdateClinicaDto dto)
    {
        await _service.UpdateAsync(id, dto);

        return NoContent();
    }

    /// <summary>
    /// Remove uma clÃ­nica.
    /// </summary>
    /// <param name="id">ID da clÃ­nica.</param>
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _service.DeleteAsync(id);

        return NoContent();
    }
}
