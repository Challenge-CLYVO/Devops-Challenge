using PetCare.Domain.Entities;

namespace PetCare.Tests.Unit.Domain.Entities;

public class AplicacaoVacinaTests
{
    [Fact]
    public void AplicacaoVacina_PropriedadesInformadas_MantemValores()
    {
        // Arrange
        var data = new DateTime(2026, 8, 27);

        var aplicacao = new AplicacaoVacina
        {
            IdAplicacao = 1,
            DataAplicacao = data,
            IdVacina = 5,
            IdPet = 10
        };

        // Act
        var result = aplicacao;

        // Assert
        Assert.Equal(1, result.IdAplicacao);
        Assert.Equal(data, result.DataAplicacao);
        Assert.Equal(5, result.IdVacina);
        Assert.Equal(10, result.IdPet);
    }
}