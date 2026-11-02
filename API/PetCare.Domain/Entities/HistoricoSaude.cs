namespace PetCare.Domain.Entities;

public class HistoricoSaude
{
    public int IdHistorico { get; set; }

    public string Descricao { get; set; } = string.Empty;

    public DateTime DataRegistro { get; set; }

    public int IdPet { get; set; }

    // Relacionamento
    public Pet Pet { get; set; } = null!;
}