using Microsoft.AspNetCore.Mvc;
using PetCare.Application.DTOs.AplicacaoVacina;
using PetCare.Application.Interfaces;

namespace PetCare.API.Controllers;

/// <summary>
/// Controller responsável pelo gerenciamento de aplicações de vacina.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class AplicacaoVacinaController : ControllerBase
{
    private readonly IAplicacaoVacinaService _service;

    public AplicacaoVacinaController(IAplicacaoVacinaService service)
    {
        _service = service;
    }

    /// <summary>
    /// Retorna todas as aplicações de vacina cadastradas.
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var aplicacoes = await _service.GetAllAsync();

        return Ok(aplicacoes);
    }

    /// <summary>
    /// Busca uma aplicação de vacina pelo ID.
    /// </summary>
    /// <param name="id">ID da aplicação.</param>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var aplicacao = await _service.GetByIdAsync(id);

        return Ok(aplicacao);
    }

    /// <summary>
    /// Cria uma nova aplicação de vacina.
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> Create(CreateAplicacaoVacinaDto dto)
    {
        await _service.CreateAsync(dto);

        return Created("", dto);
    }

    /// <summary>
    /// Atualiza uma aplicação de vacina existente.
    /// </summary>
    /// <param name="id">ID da aplicação.</param>
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, UpdateAplicacaoVacinaDto dto)
    {
        await _service.UpdateAsync(id, dto);

        return NoContent();
    }

    /// <summary>
    /// Remove uma aplicação de vacina.
    /// </summary>
    /// <param name="id">ID da aplicação.</param>
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _service.DeleteAsync(id);

        return NoContent();
    }
}