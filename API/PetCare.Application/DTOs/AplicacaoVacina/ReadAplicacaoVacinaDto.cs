namespace PetCare.Application.DTOs.AplicacaoVacina;

public class ReadAplicacaoVacinaDto
{
    public int IdAplicacao { get; set; }

    public DateTime DataAplicacao { get; set; }

    public int IdVacina { get; set; }

    public int IdPet { get; set; }
}