using AutoMapper;
using Moq;
using PetCare.Application.DTOs.Vacina;
using PetCare.Application.Exceptions;
using PetCare.Application.Interfaces;
using PetCare.Application.Services;
using PetCare.Domain.Entities;

namespace PetCare.Tests.Unit.Application.Services;

public class VacinaServiceTests
{
    [Fact]
    public async Task GetAllAsync_ExistemVacinas_RetornaListaDeDtos()
    {
        var repositoryMock = new Mock<IVacinaRepository>();
        var mapperMock = new Mock<IMapper>();

        var vacinas = new List<Vacina>
        {
            new() { IdVacina = 1 },
            new() { IdVacina = 2 }
        };

        var dtos = new List<ReadVacinaDto>
        {
            new() { IdVacina = 1 },
            new() { IdVacina = 2 }
        };

        repositoryMock
            .Setup(r => r.GetAllAsync())
            .ReturnsAsync(vacinas);

        mapperMock
            .Setup(m => m.Map<IEnumerable<ReadVacinaDto>>(vacinas))
            .Returns(dtos);

        var service = new VacinaService(
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
    public async Task GetByIdAsync_VacinaExistente_RetornaDto()
    {
        var repositoryMock = new Mock<IVacinaRepository>();
        var mapperMock = new Mock<IMapper>();

        var vacina = new Vacina { IdVacina = 1 };
        var dto = new ReadVacinaDto { IdVacina = 1 };

        repositoryMock
            .Setup(r => r.GetByIdAsync(1))
            .ReturnsAsync(vacina);

        mapperMock
            .Setup(m => m.Map<ReadVacinaDto>(vacina))
            .Returns(dto);

        var service = new VacinaService(
            repositoryMock.Object,
            mapperMock.Object);

        var resultado = await service.GetByIdAsync(1);

        Assert.NotNull(resultado);
        Assert.Equal(1, resultado!.IdVacina);
    }

    [Fact]
    public async Task GetByIdAsync_VacinaInexistente_LancaNotFoundException()
    {
        var repositoryMock = new Mock<IVacinaRepository>();
        var mapperMock = new Mock<IMapper>();

        repositoryMock
            .Setup(r => r.GetByIdAsync(999))
            .ReturnsAsync((Vacina?)null);

        var service = new VacinaService(
            repositoryMock.Object,
            mapperMock.Object);

        await Assert.ThrowsAsync<NotFoundException>(
            () => service.GetByIdAsync(999));
    }

    [Fact]
    public async Task CreateAsync_DtoValido_AdicionaVacinaNoRepositorio()
    {
        var repositoryMock = new Mock<IVacinaRepository>();
        var mapperMock = new Mock<IMapper>();

        var dto = new CreateVacinaDto
        {
            Nome = "Vacina Antirrábica",
            Descricao = "Prevenção contra a raiva"
        };

        var vacina = new Vacina { IdVacina = 1 };

        mapperMock
            .Setup(m => m.Map<Vacina>(dto))
            .Returns(vacina);

        repositoryMock
            .Setup(r => r.AddAsync(vacina))
            .Returns(Task.CompletedTask);

        var service = new VacinaService(
            repositoryMock.Object,
            mapperMock.Object);

        await service.CreateAsync(dto);

        repositoryMock.Verify(
            r => r.AddAsync(vacina),
            Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_VacinaExistente_AtualizaVacina()
    {
        var repositoryMock = new Mock<IVacinaRepository>();
        var mapperMock = new Mock<IMapper>();

        var vacina = new Vacina { IdVacina = 1 };

        var dto = new UpdateVacinaDto
        {
            Nome = "Vacina Atualizada",
            Descricao = "Descrição atualizada"
        };

        repositoryMock
            .Setup(r => r.GetByIdAsync(1))
            .ReturnsAsync(vacina);

        repositoryMock
            .Setup(r => r.UpdateAsync(vacina))
            .Returns(Task.CompletedTask);

        var service = new VacinaService(
            repositoryMock.Object,
            mapperMock.Object);

        await service.UpdateAsync(1, dto);

        mapperMock.Verify(
            m => m.Map(dto, vacina),
            Times.Once);

        repositoryMock.Verify(
            r => r.UpdateAsync(vacina),
            Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_VacinaInexistente_LancaNotFoundException()
    {
        var repositoryMock = new Mock<IVacinaRepository>();
        var mapperMock = new Mock<IMapper>();

        repositoryMock
            .Setup(r => r.GetByIdAsync(999))
            .ReturnsAsync((Vacina?)null);

        var service = new VacinaService(
            repositoryMock.Object,
            mapperMock.Object);

        await Assert.ThrowsAsync<NotFoundException>(
            () => service.UpdateAsync(999, new UpdateVacinaDto()));
    }

    [Fact]
    public async Task DeleteAsync_IdValido_ChamaRepositorio()
    {
        var repositoryMock = new Mock<IVacinaRepository>();
        var mapperMock = new Mock<IMapper>();

        repositoryMock
            .Setup(r => r.DeleteAsync(1))
            .Returns(Task.CompletedTask);

        var service = new VacinaService(
            repositoryMock.Object,
            mapperMock.Object);

        await service.DeleteAsync(1);

        repositoryMock.Verify(
            r => r.DeleteAsync(1),
            Times.Once);
    }
}
