using AutoMapper;
using Moq;
using PetCare.Application.DTOs.AplicacaoVacina;
using PetCare.Application.Exceptions;
using PetCare.Application.Interfaces;
using PetCare.Application.Services;
using PetCare.Domain.Entities;

namespace PetCare.Tests.Unit.Application.Services;

public class AplicacaoVacinaServiceTests
{
    [Fact]
    public async Task GetAllAsync_ExistemAplicacoes_RetornaListaDeDtos()
    {
        var repositoryMock = new Mock<IAplicacaoVacinaRepository>();
        var mapperMock = new Mock<IMapper>();

        var aplicacoes = new List<AplicacaoVacina>
        {
            new() { IdAplicacao = 1 },
            new() { IdAplicacao = 2 }
        };

        var dtos = new List<ReadAplicacaoVacinaDto>
        {
            new() { IdAplicacao = 1 },
            new() { IdAplicacao = 2 }
        };

        repositoryMock
            .Setup(r => r.GetAllAsync())
            .ReturnsAsync(aplicacoes);

        mapperMock
            .Setup(m => m.Map<IEnumerable<ReadAplicacaoVacinaDto>>(aplicacoes))
            .Returns(dtos);

        var service = new AplicacaoVacinaService(
            repositoryMock.Object,
            mapperMock.Object);

        var resultado = await service.GetAllAsync();

        Assert.NotNull(resultado);
        Assert.Equal(2, resultado.Count());

        repositoryMock.Verify(
            r => r.GetAllAsync(),
            Times.Once);
    }

    [Fact]
    public async Task GetByIdAsync_AplicacaoExistente_RetornaDto()
    {
        var repositoryMock = new Mock<IAplicacaoVacinaRepository>();
        var mapperMock = new Mock<IMapper>();

        var aplicacao = new AplicacaoVacina { IdAplicacao = 1 };
        var dto = new ReadAplicacaoVacinaDto { IdAplicacao = 1 };

        repositoryMock
            .Setup(r => r.GetByIdAsync(1))
            .ReturnsAsync(aplicacao);

        mapperMock
            .Setup(m => m.Map<ReadAplicacaoVacinaDto>(aplicacao))
            .Returns(dto);

        var service = new AplicacaoVacinaService(
            repositoryMock.Object,
            mapperMock.Object);

        var resultado = await service.GetByIdAsync(1);

        Assert.NotNull(resultado);
        Assert.Equal(1, resultado!.IdAplicacao);
    }

    [Fact]
    public async Task GetByIdAsync_AplicacaoInexistente_LancaNotFoundException()
    {
        var repositoryMock = new Mock<IAplicacaoVacinaRepository>();
        var mapperMock = new Mock<IMapper>();

        repositoryMock
            .Setup(r => r.GetByIdAsync(999))
            .ReturnsAsync((AplicacaoVacina?)null);

        var service = new AplicacaoVacinaService(
            repositoryMock.Object,
            mapperMock.Object);

        await Assert.ThrowsAsync<NotFoundException>(
            () => service.GetByIdAsync(999));
    }

    [Fact]
    public async Task CreateAsync_DtoValido_AdicionaAplicacaoNoRepositorio()
    {
        var repositoryMock = new Mock<IAplicacaoVacinaRepository>();
        var mapperMock = new Mock<IMapper>();

        var dto = new CreateAplicacaoVacinaDto
        {
            DataAplicacao = new DateTime(2026, 10, 16),
            IdVacina = 1,
            IdPet = 1
        };

        var aplicacao = new AplicacaoVacina { IdAplicacao = 1 };

        mapperMock
            .Setup(m => m.Map<AplicacaoVacina>(dto))
            .Returns(aplicacao);

        repositoryMock
            .Setup(r => r.AddAsync(aplicacao))
            .Returns(Task.CompletedTask);

        var service = new AplicacaoVacinaService(
            repositoryMock.Object,
            mapperMock.Object);

        await service.CreateAsync(dto);

        repositoryMock.Verify(
            r => r.AddAsync(aplicacao),
            Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_AplicacaoExistente_AtualizaAplicacao()
    {
        var repositoryMock = new Mock<IAplicacaoVacinaRepository>();
        var mapperMock = new Mock<IMapper>();

        var aplicacao = new AplicacaoVacina { IdAplicacao = 1 };

        var dto = new UpdateAplicacaoVacinaDto
        {
            DataAplicacao = new DateTime(2026, 10, 17),
            IdVacina = 2,
            IdPet = 1
        };

        repositoryMock
            .Setup(r => r.GetByIdAsync(1))
            .ReturnsAsync(aplicacao);

        repositoryMock
            .Setup(r => r.UpdateAsync(aplicacao))
            .Returns(Task.CompletedTask);

        var service = new AplicacaoVacinaService(
            repositoryMock.Object,
            mapperMock.Object);

        await service.UpdateAsync(1, dto);

        mapperMock.Verify(
            m => m.Map(dto, aplicacao),
            Times.Once);

        repositoryMock.Verify(
            r => r.UpdateAsync(aplicacao),
            Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_AplicacaoInexistente_LancaNotFoundException()
    {
        var repositoryMock = new Mock<IAplicacaoVacinaRepository>();
        var mapperMock = new Mock<IMapper>();

        repositoryMock
            .Setup(r => r.GetByIdAsync(999))
            .ReturnsAsync((AplicacaoVacina?)null);

        var service = new AplicacaoVacinaService(
            repositoryMock.Object,
            mapperMock.Object);

        await Assert.ThrowsAsync<NotFoundException>(
            () => service.UpdateAsync(
                999,
                new UpdateAplicacaoVacinaDto()));
    }

    [Fact]
    public async Task DeleteAsync_IdValido_ChamaRepositorio()
    {
        var repositoryMock = new Mock<IAplicacaoVacinaRepository>();
        var mapperMock = new Mock<IMapper>();

        repositoryMock
            .Setup(r => r.DeleteAsync(1))
            .Returns(Task.CompletedTask);

        var service = new AplicacaoVacinaService(
            repositoryMock.Object,
            mapperMock.Object);

        await service.DeleteAsync(1);

        repositoryMock.Verify(
            r => r.DeleteAsync(1),
            Times.Once);
    }
}