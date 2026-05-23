namespace PetCare.Domain.Entities;

public class Tutor
{
    public int IdTutor { get; set; }

    public string Nome { get; set; } = string.Empty;

    public string? Telefone { get; set; }

    public string? Email { get; set; }

    public ICollection<Pet> Pets { get; set; } = new List<Pet>();
}