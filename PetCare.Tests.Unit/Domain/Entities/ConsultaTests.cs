using PetCare.Domain.Entities;

namespace PetCare.Tests.Unit.Domain.Entities;

public class ConsultaTests
{
    [Fact]
    public void Consulta_PropriedadesInformadas_MantemValores()
    {
        // Arrange
        var data = new DateTime(2026, 8, 27);

        var consulta = new Consulta
        {
            IdConsulta = 1,
            DataConsulta = data,
            Descricao = "Consulta de rotina",
            IdPet = 10,
            IdClinica = 20
        };

        // Act
        var result = consulta;

        // Assert
        Assert.Equal(1, result.IdConsulta);
        Assert.Equal(data, result.DataConsulta);
        Assert.Equal("Consulta de rotina", result.Descricao);
        Assert.Equal(10, result.IdPet);
        Assert.Equal(20, result.IdClinica);
    }
}