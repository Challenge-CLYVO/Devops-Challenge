namespace PetCare.Domain.Entities;

public class Clinica
{
    public int IdClinica { get; set; }

    public string Nome { get; set; } = string.Empty;

    public string Endereco { get; set; } = string.Empty;

    public string? Telefone { get; set; }

    // Relacionamentos
    public ICollection<Consulta> Consultas { get; set; } = new List<Consulta>();
}