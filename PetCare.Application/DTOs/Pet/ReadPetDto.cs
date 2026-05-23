namespace PetCare.Application.DTOs.Pet;

public class ReadPetDto
{
    public int IdPet { get; set; }

    public string Nome { get; set; } = string.Empty;

    public int? Idade { get; set; }

    public string Especie { get; set; } = string.Empty;

    public string? Raca { get; set; }

    public int IdTutor { get; set; }

    public string NomeTutor { get; set; } = string.Empty;
}