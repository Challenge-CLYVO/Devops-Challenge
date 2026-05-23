using System.ComponentModel.DataAnnotations;

namespace PetCare.Application.DTOs.HistoricoSaude;

public class CreateHistoricoSaudeDto
{
    [Required(ErrorMessage = "A descrição é obrigatória.")]
    [StringLength(255, ErrorMessage = "A descrição deve ter no máximo 255 caracteres.")]
    public string Descricao { get; set; } = string.Empty;

    [Required(ErrorMessage = "A data do registro é obrigatória.")]
    public DateTime DataRegistro { get; set; }

    [Required(ErrorMessage = "O id do pet é obrigatório.")]
    public int IdPet { get; set; }
}