using PetCare.Domain.Entities;

namespace PetCare.Tests.Unit.Domain.Entities;

public class VacinaTests
{
    [Fact]
    public void Vacina_Instanciada_InicializaColecaoDeAplicacoes()
    {
        // Arrange

        // Act
        var vacina = new Vacina();

        // Assert
        Assert.NotNull(vacina.AplicacoesVacina);
    }

    [Fact]
    public void Vacina_PropriedadesInformadas_MantemValores()
    {
        // Arrange
        var vacina = new Vacina
        {
            IdVacina = 1,
            Nome = "Antirrábica",
            Descricao = "Vacina contra raiva"
        };

        // Act
        var result = vacina;

        // Assert
        Assert.Equal(1, result.IdVacina);
        Assert.Equal("Antirrábica", result.Nome);
        Assert.Equal("Vacina contra raiva", result.Descricao);
    }
}