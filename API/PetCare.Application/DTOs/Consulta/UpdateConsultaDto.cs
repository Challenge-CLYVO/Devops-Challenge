using System.ComponentModel.DataAnnotations;

namespace PetCare.Application.DTOs.Consulta;

public class UpdateConsultaDto
{
    [Required(ErrorMessage = "A data da consulta é obrigatória.")]
    public DateTime DataConsulta { get; set; }

    [StringLength(
        200,
        ErrorMessage = "A descrição deve ter no máximo 200 caracteres.")]
    public string? Descricao { get; set; }

    [Required(ErrorMessage = "O id do pet é obrigatório.")]
    [Range(
        1,
        int.MaxValue,
        ErrorMessage = "O id do pet deve ser maior que zero.")]
    public int IdPet { get; set; }

    [Required(ErrorMessage = "O id da clínica é obrigatório.")]
    [Range(
        1,
        int.MaxValue,
        ErrorMessage = "O id da clínica deve ser maior que zero.")]
    public int IdClinica { get; set; }
}