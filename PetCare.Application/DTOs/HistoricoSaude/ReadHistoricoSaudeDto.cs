namespace PetCare.Application.DTOs.HistoricoSaude;

public class ReadHistoricoSaudeDto
{
    public int IdHistorico { get; set; }

    public string Descricao { get; set; } = string.Empty;

    public DateTime DataRegistro { get; set; }

    public int IdPet { get; set; }
}