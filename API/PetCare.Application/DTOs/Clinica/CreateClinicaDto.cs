using System.ComponentModel.DataAnnotations;

namespace PetCare.Application.DTOs.Clinica;

public class CreateClinicaDto
{
    [Required(ErrorMessage = "O nome da clínica é obrigatório.")]
    [StringLength(100, ErrorMessage = "O nome da clínica deve ter no máximo 100 caracteres.")]
    public string Nome { get; set; } = string.Empty;

    [Required(ErrorMessage = "O endereço é obrigatório.")]
    [StringLength(200, ErrorMessage = "O endereço deve ter no máximo 200 caracteres.")]
    public string Endereco { get; set; } = string.Empty;

    [Required(ErrorMessage = "O telefone é obrigatório.")]
    [Phone(ErrorMessage = "Telefone inválido.")]
    [StringLength(20, ErrorMessage = "O telefone deve ter no máximo 20 caracteres.")]
    public string Telefone { get; set; } = string.Empty;
}