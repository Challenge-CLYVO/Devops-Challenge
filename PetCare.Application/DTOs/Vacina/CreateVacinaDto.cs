using System.ComponentModel.DataAnnotations;

namespace PetCare.Application.DTOs.Vacina;

public class CreateVacinaDto
{
    [Required(ErrorMessage = "O nome da vacina é obrigatório.")]
    [StringLength(100, ErrorMessage = "O nome da vacina deve ter no máximo 100 caracteres.")]
    public string Nome { get; set; } = string.Empty;

    [StringLength(200, ErrorMessage = "A descrição deve ter no máximo 200 caracteres.")]
    public string? Descricao { get; set; }
}