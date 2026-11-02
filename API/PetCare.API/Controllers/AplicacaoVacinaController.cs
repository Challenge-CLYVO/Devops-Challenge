using Microsoft.AspNetCore.Mvc;
using PetCare.Application.DTOs.AplicacaoVacina;
using PetCare.Application.Interfaces;

namespace PetCare.API.Controllers;

/// <summary>
/// Controller responsÃ¡vel pelo gerenciamento de aplicaÃ§Ãµes de vacina.
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
    /// Retorna todas as aplicaÃ§Ãµes de vacina cadastradas.
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var aplicacoes = await _service.GetAllAsync();

        return Ok(aplicacoes);
    }

    /// <summary>
    /// Busca uma aplicaÃ§Ã£o de vacina pelo ID.
    /// </summary>
    /// <param name="id">ID da aplicaÃ§Ã£o.</param>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var aplicacao = await _service.GetByIdAsync(id);

        return Ok(aplicacao);
    }

    /// <summary>
    /// Cria uma nova aplicaÃ§Ã£o de vacina.
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> Create(CreateAplicacaoVacinaDto dto)
    {
        await _service.CreateAsync(dto);

        return Created("", dto);
    }

    /// <summary>
    /// Atualiza uma aplicaÃ§Ã£o de vacina existente.
    /// </summary>
    /// <param name="id">ID da aplicaÃ§Ã£o.</param>
    /// <param name="dto">Dados atualizados da aplicação de vacina.</param>
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, UpdateAplicacaoVacinaDto dto)
    {
        await _service.UpdateAsync(id, dto);

        return NoContent();
    }

    /// <summary>
    /// Remove uma aplicaÃ§Ã£o de vacina.
    /// </summary>
    /// <param name="id">ID da aplicaÃ§Ã£o.</param>
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _service.DeleteAsync(id);

        return NoContent();
    }
}
