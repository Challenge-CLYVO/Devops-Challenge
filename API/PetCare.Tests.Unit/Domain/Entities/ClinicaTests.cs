using PetCare.Domain.Entities;

namespace PetCare.Tests.Unit.Domain.Entities;

public class ClinicaTests
{
    [Fact]
    public void Clinica_Instanciada_InicializaColecaoDeConsultas()
    {
        // Arrange

        // Act
        var clinica = new Clinica();

        // Assert
        Assert.NotNull(clinica.Consultas);
    }

    [Fact]
    public void Clinica_PropriedadesInformadas_MantemValores()
    {
        // Arrange
        var clinica = new Clinica
        {
            IdClinica = 1,
            Nome = "PetCare Clínica",
            Endereco = "Rua Principal",
            Telefone = "11999999999"
        };

        // Act
        var result = clinica;

        // Assert
        Assert.Equal(1, result.IdClinica);
        Assert.Equal("PetCare Clínica", result.Nome);
        Assert.Equal("Rua Principal", result.Endereco);
        Assert.Equal("11999999999", result.Telefone);
    }
}