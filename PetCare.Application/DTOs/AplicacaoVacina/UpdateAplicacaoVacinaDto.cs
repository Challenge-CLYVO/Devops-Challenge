using System.ComponentModel.DataAnnotations;

namespace PetCare.Application.DTOs.AplicacaoVacina;

public class UpdateAplicacaoVacinaDto
{
    [Required(ErrorMessage = "A data da aplicação é obrigatória.")]
    public DateTime DataAplicacao { get; set; }

    [Required(ErrorMessage = "O id da vacina é obrigatório.")]
    public int IdVacina { get; set; }

    [Required(ErrorMessage = "O id do pet é obrigatório.")]
    public int IdPet { get; set; }
}