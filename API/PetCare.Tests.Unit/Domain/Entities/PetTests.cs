using PetCare.Domain.Entities;

namespace PetCare.Tests.Unit.Domain.Entities;

public class PetTests
{
    [Fact]
    public void Pet_Instanciado_InicializaColecoes()
    {
        // Arrange
        // Act
        var pet = new Pet();

        // Assert
        Assert.NotNull(pet.Consultas);
        Assert.NotNull(pet.HistoricosSaude);
        Assert.NotNull(pet.AplicacoesVacina);
    }

    [Fact]
    public void Pet_PropriedadesInformadas_MantemValores()
    {
        // Arrange
        var pet = new Pet
        {
            IdPet = 1,
            Nome = "Rex",
            Idade = 5,
            Especie = "Cachorro",
            Raca = "Labrador",
            IdTutor = 10
        };

        // Act
        var result = pet;

        // Assert
        Assert.Equal(1, result.IdPet);
        Assert.Equal("Rex", result.Nome);
        Assert.Equal(5, result.Idade);
        Assert.Equal("Cachorro", result.Especie);
        Assert.Equal("Labrador", result.Raca);
        Assert.Equal(10, result.IdTutor);
    }
}