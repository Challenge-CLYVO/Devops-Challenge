namespace PetCare.Domain.Entities;

public class Vacina
{
    public int IdVacina { get; set; }

    public string Nome { get; set; } = string.Empty;

    public string? Descricao { get; set; }

    // Relacionamentos
    public ICollection<AplicacaoVacina> AplicacoesVacina { get; set; } = new List<AplicacaoVacina>();
}