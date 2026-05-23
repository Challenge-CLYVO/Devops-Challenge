using Microsoft.AspNetCore.Mvc;
using PetCare.Application.DTOs.Consulta;
using PetCare.Application.Interfaces;

namespace PetCare.API.Controllers;

/// <summary>
/// Controller responsável pelo gerenciamento de consultas.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class ConsultaController : ControllerBase
{
    private readonly IConsultaService _service;

    public ConsultaController(IConsultaService service)
    {
        _service = service;
    }

    /// <summary>
    /// Retorna todas as consultas cadastradas.
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var consultas = await _service.GetAllAsync();

        return Ok(consultas);
    }

    /// <summary>
    /// Busca uma consulta pelo ID.
    /// </summary>
    /// <param name="id">ID da consulta.</param>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var consulta = await _service.GetByIdAsync(id);

        return Ok(consulta);
    }

    /// <summary>
    /// Cria uma nova consulta.
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> Create(CreateConsultaDto dto)
    {
        await _service.CreateAsync(dto);

        return Created("", dto);
    }

    /// <summary>
    /// Atualiza uma consulta existente.
    /// </summary>
    /// <param name="id">ID da consulta.</param>
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, UpdateConsultaDto dto)
    {
        await _service.UpdateAsync(id, dto);

        return NoContent();
    }

    /// <summary>
    /// Remove uma consulta.
    /// </summary>
    /// <param name="id">ID da consulta.</param>
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _service.DeleteAsync(id);

        return NoContent();
    }
}