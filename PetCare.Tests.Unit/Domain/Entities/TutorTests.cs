using PetCare.Domain.Entities;

namespace PetCare.Tests.Unit.Domain.Entities;

public class TutorTests
{
    [Fact]
    public void Tutor_Instanciado_InicializaColecaoDePets()
    {
        // Arrange

        // Act
        var tutor = new Tutor();

        // Assert
        Assert.NotNull(tutor.Pets);
    }

    [Fact]
    public void Tutor_PropriedadesInformadas_MantemValores()
    {
        // Arrange
        var tutor = new Tutor
        {
            IdTutor = 1,
            Nome = "Lucas",
            Telefone = "11999999999",
            Email = "lucas@email.com"
        };

        // Act
        var result = tutor;

        // Assert
        Assert.Equal(1, result.IdTutor);
        Assert.Equal("Lucas", result.Nome);
        Assert.Equal("11999999999", result.Telefone);
        Assert.Equal("lucas@email.com", result.Email);
    }
}