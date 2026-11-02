using AutoMapper;
using Moq;
using PetCare.Application.DTOs.HistoricoSaude;
using PetCare.Application.Exceptions;
using PetCare.Application.Interfaces;
using PetCare.Application.Services;
using PetCare.Domain.Entities;

namespace PetCare.Tests.Unit.Application.Services;

public class HistoricoSaudeServiceTests
{
    private readonly Mock<IHistoricoSaudeRepository> _repositoryMock = new();
    private readonly Mock<IMapper> _mapperMock = new();

    private HistoricoSaudeService CriarService()
    {
        return new HistoricoSaudeService(
            _repositoryMock.Object,
            _mapperMock.Object);
    }

    [Fact]
    public async Task GetAllAsync_ComRegistros_RetornaListaDeDtos()
    {
        var historicos = new List<HistoricoSaude>
        {
            new() { IdHistorico = 1, Descricao = "Consulta de rotina" },
            new() { IdHistorico = 2, Descricao = "Exame de sangue" }
        };

        var dtos = new List<ReadHistoricoSaudeDto>
        {
            new() { IdHistorico = 1, Descricao = "Consulta de rotina" },
            new() { IdHistorico = 2, Descricao = "Exame de sangue" }
        };

        _repositoryMock
            .Setup(r => r.GetAllAsync())
            .ReturnsAsync(historicos);

        _mapperMock
            .Setup(m => m.Map<IEnumerable<ReadHistoricoSaudeDto>>(historicos))
            .Returns(dtos);

        var service = CriarService();

        var resultado = await service.GetAllAsync();

        Assert.NotNull(resultado);
        Assert.Equal(2, resultado.Count());
        _repositoryMock.Verify(r => r.GetAllAsync(), Times.Once);
    }

    [Fact]
    public async Task GetByIdAsync_RegistroExistente_RetornaDto()
    {
        var historico = new HistoricoSaude
        {
            IdHistorico = 1,
            Descricao = "Consulta de rotina"
        };

        var dto = new ReadHistoricoSaudeDto
        {
            IdHistorico = 1,
            Descricao = "Consulta de rotina"
        };

        _repositoryMock
            .Setup(r => r.GetByIdAsync(1))
            .ReturnsAsync(historico);

        _mapperMock
            .Setup(m => m.Map<ReadHistoricoSaudeDto>(historico))
            .Returns(dto);

        var service = CriarService();

        var resultado = await service.GetByIdAsync(1);

        Assert.NotNull(resultado);
        Assert.Equal(1, resultado!.IdHistorico);
        Assert.Equal("Consulta de rotina", resultado.Descricao);
    }

    [Fact]
    public async Task GetByIdAsync_RegistroInexistente_LancaNotFoundException()
    {
        _repositoryMock
            .Setup(r => r.GetByIdAsync(999))
            .ReturnsAsync((HistoricoSaude?)null);

        var service = CriarService();

        var act = () => service.GetByIdAsync(999);

        await Assert.ThrowsAsync<NotFoundException>(act);
    }

    [Fact]
    public async Task CreateAsync_DtoValido_AdicionaRegistro()
    {
        var dto = new CreateHistoricoSaudeDto
        {
            Descricao = "Vacinação registrada",
            DataRegistro = new DateTime(2026, 10, 16),
            IdPet = 1
        };

        var historico = new HistoricoSaude
        {
            IdHistorico = 1,
            Descricao = dto.Descricao,
            DataRegistro = dto.DataRegistro,
            IdPet = dto.IdPet
        };

        _mapperMock
            .Setup(m => m.Map<HistoricoSaude>(dto))
            .Returns(historico);

        _repositoryMock
            .Setup(r => r.AddAsync(historico))
            .Returns(Task.CompletedTask);

        var service = CriarService();

        await service.CreateAsync(dto);

        _mapperMock.Verify(
            m => m.Map<HistoricoSaude>(dto),
            Times.Once);

        _repositoryMock.Verify(
            r => r.AddAsync(historico),
            Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_RegistroExistente_AtualizaRegistro()
    {
        var historico = new HistoricoSaude
        {
            IdHistorico = 1,
            Descricao = "Descrição antiga"
        };

        var dto = new UpdateHistoricoSaudeDto
        {
            Descricao = "Descrição atualizada",
            DataRegistro = new DateTime(2026, 10, 16),
            IdPet = 1
        };

        _repositoryMock
            .Setup(r => r.GetByIdAsync(1))
            .ReturnsAsync(historico);

        _mapperMock
            .Setup(m => m.Map(dto, historico));

        _repositoryMock
            .Setup(r => r.UpdateAsync(historico))
            .Returns(Task.CompletedTask);

        var service = CriarService();

        await service.UpdateAsync(1, dto);

        _mapperMock.Verify(
            m => m.Map(dto, historico),
            Times.Once);

        _repositoryMock.Verify(
            r => r.UpdateAsync(historico),
            Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_RegistroInexistente_LancaNotFoundException()
    {
        _repositoryMock
            .Setup(r => r.GetByIdAsync(999))
            .ReturnsAsync((HistoricoSaude?)null);

        var service = CriarService();

        var act = () => service.UpdateAsync(
            999,
            new UpdateHistoricoSaudeDto
            {
                Descricao = "Descrição atualizada",
                DataRegistro = new DateTime(2026, 10, 16),
                IdPet = 1
            });

        await Assert.ThrowsAsync<NotFoundException>(act);
    }

    [Fact]
    public async Task DeleteAsync_IdValido_ChamaRepositorio()
    {
        _repositoryMock
            .Setup(r => r.DeleteAsync(1))
            .Returns(Task.CompletedTask);

        var service = CriarService();

        await service.DeleteAsync(1);

        _repositoryMock.Verify(
            r => r.DeleteAsync(1),
            Times.Once);
    }
}
