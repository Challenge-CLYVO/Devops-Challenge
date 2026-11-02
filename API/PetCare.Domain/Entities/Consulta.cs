namespace PetCare.Domain.Entities;

public class Consulta
{
    public int IdConsulta { get; set; }

    public DateTime DataConsulta { get; set; }

    public string? Descricao { get; set; }

    public int IdPet { get; set; }

    public int IdClinica { get; set; }

    // Relacionamentos
    public Pet Pet { get; set; } = null!;

    public Clinica Clinica { get; set; } = null!;
}