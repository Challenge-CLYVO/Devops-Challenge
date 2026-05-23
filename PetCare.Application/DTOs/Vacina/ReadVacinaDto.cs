namespace PetCare.Application.DTOs.Vacina;

public class ReadVacinaDto
{
    public int IdVacina { get; set; }

    public string Nome { get; set; } = string.Empty;

    public string? Descricao { get; set; }
}