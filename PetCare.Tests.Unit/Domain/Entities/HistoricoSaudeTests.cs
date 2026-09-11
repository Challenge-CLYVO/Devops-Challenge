using PetCare.Domain.Entities;

namespace PetCare.Tests.Unit.Domain.Entities;

public class HistoricoSaudeTests
{
    [Fact]
    public void HistoricoSaude_PropriedadesInformadas_MantemValores()
    {
        // Arrange
        var data = new DateTime(2026, 8, 27);

        var historico = new HistoricoSaude
        {
            IdHistorico = 1,
            Descricao = "Animal apresentou melhora",
            DataRegistro = data,
            IdPet = 10
        };

        // Act
        var result = historico;

        // Assert
        Assert.Equal(1, result.IdHistorico);
        Assert.Equal("Animal apresentou melhora", result.Descricao);
        Assert.Equal(data, result.DataRegistro);
        Assert.Equal(10, result.IdPet);
    }
}