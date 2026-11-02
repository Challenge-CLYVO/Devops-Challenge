using Microsoft.AspNetCore.Mvc;
using PetCare.Application.DTOs.HistoricoSaude;
using PetCare.Application.Interfaces;

namespace PetCare.API.Controllers;

/// <summary>
/// Controller responsÃ¡vel pelo gerenciamento do histÃ³rico de saÃºde.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class HistoricoSaudeController : ControllerBase
{
    private readonly IHistoricoSaudeService _service;

    public HistoricoSaudeController(IHistoricoSaudeService service)
    {
        _service = service;
    }

    /// <summary>
    /// Retorna todos os histÃ³ricos de saÃºde cadastrados.
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var historicos = await _service.GetAllAsync();

        return Ok(historicos);
    }

    /// <summary>
    /// Busca um histÃ³rico de saÃºde pelo ID.
    /// </summary>
    /// <param name="id">ID do histÃ³rico.</param>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var historico = await _service.GetByIdAsync(id);

        return Ok(historico);
    }

    /// <summary>
    /// Cria um novo histÃ³rico de saÃºde.
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> Create(CreateHistoricoSaudeDto dto)
    {
        await _service.CreateAsync(dto);

        return Created("", dto);
    }

    /// <summary>
    /// Atualiza um histÃ³rico de saÃºde existente.
    /// </summary>
    /// <param name="id">ID do histÃ³rico.</param>
    /// <param name="dto">Dados atualizados do histórico de saúde.</param>
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, UpdateHistoricoSaudeDto dto)
    {
        await _service.UpdateAsync(id, dto);

        return NoContent();
    }

    /// <summary>
    /// Remove um histÃ³rico de saÃºde.
    /// </summary>
    /// <param name="id">ID do histÃ³rico.</param>
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _service.DeleteAsync(id);

        return NoContent();
    }
}
