using System.ComponentModel.DataAnnotations;

namespace PetCare.Application.DTOs.Tutor;

public class CreateTutorDto
{
    [Required(ErrorMessage = "O nome do tutor é obrigatório.")]
    [StringLength(100, ErrorMessage = "O nome do tutor deve ter no máximo 100 caracteres.")]
    public string Nome { get; set; } = string.Empty;

    [Required(ErrorMessage = "O telefone é obrigatório.")]
    [Phone(ErrorMessage = "Telefone inválido.")]
    [StringLength(20, ErrorMessage = "O telefone deve ter no máximo 20 caracteres.")]
    public string Telefone { get; set; } = string.Empty;

    [Required(ErrorMessage = "O email é obrigatório.")]
    [EmailAddress(ErrorMessage = "Email inválido.")]
    [StringLength(100, ErrorMessage = "O email deve ter no máximo 100 caracteres.")]
    public string Email { get; set; } = string.Empty;
}