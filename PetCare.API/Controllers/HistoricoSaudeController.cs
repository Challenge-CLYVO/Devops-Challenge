using Microsoft.AspNetCore.Mvc;
using PetCare.Application.DTOs.HistoricoSaude;
using PetCare.Application.Interfaces;

namespace PetCare.API.Controllers;

/// <summary>
/// Controller responsável pelo gerenciamento do histórico de saúde.
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
    /// Retorna todos os históricos de saúde cadastrados.
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var historicos = await _service.GetAllAsync();

        return Ok(historicos);
    }

    /// <summary>
    /// Busca um histórico de saúde pelo ID.
    /// </summary>
    /// <param name="id">ID do histórico.</param>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var historico = await _service.GetByIdAsync(id);

        return Ok(historico);
    }

    /// <summary>
    /// Cria um novo histórico de saúde.
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> Create(CreateHistoricoSaudeDto dto)
    {
        await _service.CreateAsync(dto);

        return Created("", dto);
    }

    /// <summary>
    /// Atualiza um histórico de saúde existente.
    /// </summary>
    /// <param name="id">ID do histórico.</param>
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, UpdateHistoricoSaudeDto dto)
    {
        await _service.UpdateAsync(id, dto);

        return NoContent();
    }

    /// <summary>
    /// Remove um histórico de saúde.
    /// </summary>
    /// <param name="id">ID do histórico.</param>
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _service.DeleteAsync(id);

        return NoContent();
    }
}