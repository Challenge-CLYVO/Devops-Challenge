using System.ComponentModel.DataAnnotations;

namespace PetCare.Application.DTOs.Pet;

public class UpdatePetDto
{
    [Required(ErrorMessage = "O nome do pet é obrigatório.")]
    [StringLength(100, ErrorMessage = "O nome do pet deve ter no máximo 100 caracteres.")]
    public string Nome { get; set; } = string.Empty;

    [Range(0, 50, ErrorMessage = "A idade deve estar entre 0 e 50 anos.")]
    public int? Idade { get; set; }

    [Required(ErrorMessage = "A espécie é obrigatória.")]
    [StringLength(50, ErrorMessage = "A espécie deve ter no máximo 50 caracteres.")]
    public string Especie { get; set; } = string.Empty;

    [StringLength(50, ErrorMessage = "A raça deve ter no máximo 50 caracteres.")]
    public string? Raca { get; set; }

    [Required(ErrorMessage = "O id do tutor é obrigatório.")]
    public int IdTutor { get; set; }
}