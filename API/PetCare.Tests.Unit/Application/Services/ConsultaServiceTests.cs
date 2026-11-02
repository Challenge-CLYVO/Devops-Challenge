using AutoMapper;
using Moq;
using PetCare.Application.DTOs.Consulta;
using PetCare.Application.Exceptions;
using PetCare.Application.Interfaces;
using PetCare.Application.Services;
using PetCare.Domain.Entities;

namespace PetCare.Tests.Unit.Application.Services;

public class ConsultaServiceTests
{
    [Fact]
    public async Task GetAllAsync_ExistemConsultas_RetornaListaDeDtos()
    {
        var repositoryMock = new Mock<IConsultaRepository>();
        var mapperMock = new Mock<IMapper>();

        var consultas = new List<Consulta>
        {
            new() { IdConsulta = 1 },
            new() { IdConsulta = 2 }
        };

        var dtos = new List<ReadConsultaDto>
        {
            new() { IdConsulta = 1 },
            new() { IdConsulta = 2 }
        };

        repositoryMock
            .Setup(r => r.GetAllAsync())
            .ReturnsAsync(consultas);

        mapperMock
            .Setup(m => m.Map<IEnumerable<ReadConsultaDto>>(consultas))
            .Returns(dtos);

        var service = new ConsultaService(
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
    public async Task GetByIdAsync_ConsultaExistente_RetornaDto()
    {
        var repositoryMock = new Mock<IConsultaRepository>();
        var mapperMock = new Mock<IMapper>();

        var consulta = new Consulta { IdConsulta = 1 };
        var dto = new ReadConsultaDto { IdConsulta = 1 };

        repositoryMock
            .Setup(r => r.GetByIdAsync(1))
            .ReturnsAsync(consulta);

        mapperMock
            .Setup(m => m.Map<ReadConsultaDto>(consulta))
            .Returns(dto);

        var service = new ConsultaService(
            repositoryMock.Object,
            mapperMock.Object);

        var resultado = await service.GetByIdAsync(1);

        Assert.NotNull(resultado);
        Assert.Equal(1, resultado!.IdConsulta);
    }

    [Fact]
    public async Task GetByIdAsync_ConsultaInexistente_LancaNotFoundException()
    {
        var repositoryMock = new Mock<IConsultaRepository>();
        var mapperMock = new Mock<IMapper>();

        repositoryMock
            .Setup(r => r.GetByIdAsync(999))
            .ReturnsAsync((Consulta?)null);

        var service = new ConsultaService(
            repositoryMock.Object,
            mapperMock.Object);

        await Assert.ThrowsAsync<NotFoundException>(
            () => service.GetByIdAsync(999));
    }

    [Fact]
    public async Task CreateAsync_DtoValido_AdicionaConsultaNoRepositorio()
    {
        var repositoryMock = new Mock<IConsultaRepository>();
        var mapperMock = new Mock<IMapper>();

        var dto = new CreateConsultaDto
        {
            DataConsulta = new DateTime(2026, 10, 16),
            Descricao = "Consulta de rotina",
            IdPet = 1,
            IdClinica = 1
        };

        var consulta = new Consulta { IdConsulta = 1 };

        mapperMock
            .Setup(m => m.Map<Consulta>(dto))
            .Returns(consulta);

        repositoryMock
            .Setup(r => r.AddAsync(consulta))
            .Returns(Task.CompletedTask);

        var service = new ConsultaService(
            repositoryMock.Object,
            mapperMock.Object);

        await service.CreateAsync(dto);

        repositoryMock.Verify(
            r => r.AddAsync(consulta),
            Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_ConsultaExistente_AtualizaConsulta()
    {
        var repositoryMock = new Mock<IConsultaRepository>();
        var mapperMock = new Mock<IMapper>();

        var consulta = new Consulta { IdConsulta = 1 };

        var dto = new UpdateConsultaDto
        {
            DataConsulta = new DateTime(2026, 10, 17),
            Descricao = "Consulta atualizada",
            IdPet = 1,
            IdClinica = 1
        };

        repositoryMock
            .Setup(r => r.GetByIdAsync(1))
            .ReturnsAsync(consulta);

        repositoryMock
            .Setup(r => r.UpdateAsync(consulta))
            .Returns(Task.CompletedTask);

        var service = new ConsultaService(
            repositoryMock.Object,
            mapperMock.Object);

        await service.UpdateAsync(1, dto);

        mapperMock.Verify(
            m => m.Map(dto, consulta),
            Times.Once);

        repositoryMock.Verify(
            r => r.UpdateAsync(consulta),
            Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_ConsultaInexistente_LancaNotFoundException()
    {
        var repositoryMock = new Mock<IConsultaRepository>();
        var mapperMock = new Mock<IMapper>();

        repositoryMock
            .Setup(r => r.GetByIdAsync(999))
            .ReturnsAsync((Consulta?)null);

        var service = new ConsultaService(
            repositoryMock.Object,
            mapperMock.Object);

        await Assert.ThrowsAsync<NotFoundException>(
            () => service.UpdateAsync(999, new UpdateConsultaDto()));
    }

    [Fact]
    public async Task DeleteAsync_IdValido_ChamaRepositorio()
    {
        var repositoryMock = new Mock<IConsultaRepository>();
        var mapperMock = new Mock<IMapper>();

        repositoryMock
            .Setup(r => r.DeleteAsync(1))
            .Returns(Task.CompletedTask);

        var service = new ConsultaService(
            repositoryMock.Object,
            mapperMock.Object);

        await service.DeleteAsync(1);

        repositoryMock.Verify(
            r => r.DeleteAsync(1),
            Times.Once);
    }
}
