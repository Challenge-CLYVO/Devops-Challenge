namespace PetCare.Domain.Entities;

public class Pet
{
    public int IdPet { get; set; }

    public string Nome { get; set; } = string.Empty;

    public int? Idade { get; set; }

    public string Especie { get; set; } = string.Empty;

    public string? Raca { get; set; }

    public int IdTutor { get; set; }

    public Tutor Tutor { get; set; } = null!;

    public ICollection<Consulta> Consultas { get; set; } = new List<Consulta>();

    public ICollection<HistoricoSaude> HistoricosSaude { get; set; } = new List<HistoricoSaude>();

    public ICollection<AplicacaoVacina> AplicacoesVacina { get; set; } = new List<AplicacaoVacina>();
}