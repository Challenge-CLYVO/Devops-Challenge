using System.ComponentModel.DataAnnotations;

namespace PetCare.Application.DTOs.AplicacaoVacina;

public class CreateAplicacaoVacinaDto
{
    [Required(ErrorMessage = "A data da aplicação é obrigatória.")]
    public DateTime DataAplicacao { get; set; }

    [Required(ErrorMessage = "O id da vacina é obrigatório.")]
    [Range(
        1,
        int.MaxValue,
        ErrorMessage = "O id da vacina deve ser maior que zero.")]
    public int IdVacina { get; set; }

    [Required(ErrorMessage = "O id do pet é obrigatório.")]
    [Range(
        1,
        int.MaxValue,
        ErrorMessage = "O id do pet deve ser maior que zero.")]
    public int IdPet { get; set; }
}