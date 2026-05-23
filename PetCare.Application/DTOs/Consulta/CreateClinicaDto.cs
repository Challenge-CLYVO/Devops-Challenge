using System.ComponentModel.DataAnnotations;

namespace PetCare.Application.DTOs.Consulta;

public class CreateConsultaDto
{
    [Required(ErrorMessage = "A data da consulta é obrigatória.")]
    public DateTime DataConsulta { get; set; }

    [StringLength(200, ErrorMessage = "A descrição deve ter no máximo 200 caracteres.")]
    public string? Descricao { get; set; }

    [Required(ErrorMessage = "O id do pet é obrigatório.")]
    public int IdPet { get; set; }

    [Required(ErrorMessage = "O id da clínica é obrigatório.")]
    public int IdClinica { get; set; }
}