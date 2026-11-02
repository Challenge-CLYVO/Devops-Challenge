namespace PetCare.Domain.Entities;

public class AplicacaoVacina
{
    public int IdAplicacao { get; set; }

    public DateTime DataAplicacao { get; set; }

    public int IdVacina { get; set; }

    public int IdPet { get; set; }

    // Relacionamentos
    public Vacina Vacina { get; set; } = null!;

    public Pet Pet { get; set; } = null!;
}