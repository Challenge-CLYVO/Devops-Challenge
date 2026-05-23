namespace PetCare.Application.DTOs.Clinica;

public class ReadClinicaDto
{
    public int IdClinica { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string Endereco { get; set; } = string.Empty;
    public string? Telefone { get; set; }
}