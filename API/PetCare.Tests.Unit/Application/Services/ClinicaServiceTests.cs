using AutoMapper;
using Moq;
using PetCare.Application.DTOs.Clinica;
using PetCare.Application.Exceptions;
using PetCare.Application.Interfaces;
using PetCare.Application.Services;
using PetCare.Domain.Entities;

namespace PetCare.Tests.Unit.Application.Services;

public class ClinicaServiceTests
{
    [Fact]
    public async Task GetAllAsync_ExistemClinicas_RetornaListaDeDtos()
    {
        var repositoryMock = new Mock<IClinicaRepository>();
        var mapperMock = new Mock<IMapper>();

        var clinicas = new List<Clinica>
        {
            new() { IdClinica = 1 },
            new() { IdClinica = 2 }
        };

        var dtos = new List<ReadClinicaDto>
        {
            new() { IdClinica = 1 },
            new() { IdClinica = 2 }
        };

        repositoryMock
            .Setup(r => r.GetAllAsync())
            .ReturnsAsync(clinicas);

        mapperMock
            .Setup(m => m.Map<IEnumerable<ReadClinicaDto>>(clinicas))
            .Returns(dtos);

        var service = new ClinicaService(
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
    public async Task GetByIdAsync_ClinicaExistente_RetornaDto()
    {
        var repositoryMock = new Mock<IClinicaRepository>();
        var mapperMock = new Mock<IMapper>();

        var clinica = new Clinica { IdClinica = 1 };
        var dto = new ReadClinicaDto { IdClinica = 1 };

        repositoryMock
            .Setup(r => r.GetByIdAsync(1))
            .ReturnsAsync(clinica);

        mapperMock
            .Setup(m => m.Map<ReadClinicaDto>(clinica))
            .Returns(dto);

        var service = new ClinicaService(
            repositoryMock.Object,
            mapperMock.Object);

        var resultado = await service.GetByIdAsync(1);

        Assert.NotNull(resultado);
        Assert.Equal(1, resultado!.IdClinica);
    }

    [Fact]
    public async Task GetByIdAsync_ClinicaInexistente_LancaNotFoundException()
    {
        var repositoryMock = new Mock<IClinicaRepository>();
        var mapperMock = new Mock<IMapper>();

        repositoryMock
            .Setup(r => r.GetByIdAsync(999))
            .ReturnsAsync((Clinica?)null);

        var service = new ClinicaService(
            repositoryMock.Object,
            mapperMock.Object);

        await Assert.ThrowsAsync<NotFoundException>(
            () => service.GetByIdAsync(999));
    }

    [Fact]
    public async Task CreateAsync_DtoValido_AdicionaClinicaNoRepositorio()
    {
        var repositoryMock = new Mock<IClinicaRepository>();
        var mapperMock = new Mock<IMapper>();

        var dto = new CreateClinicaDto
        {
            Nome = "Clínica Pet",
            Endereco = "Rua das Flores, 100",
            Telefone = "11999999999"
        };

        var clinica = new Clinica { IdClinica = 1 };

        mapperMock
            .Setup(m => m.Map<Clinica>(dto))
            .Returns(clinica);

        repositoryMock
            .Setup(r => r.AddAsync(clinica))
            .Returns(Task.CompletedTask);

        var service = new ClinicaService(
            repositoryMock.Object,
            mapperMock.Object);

        await service.CreateAsync(dto);

        repositoryMock.Verify(
            r => r.AddAsync(clinica),
            Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_ClinicaExistente_AtualizaClinica()
    {
        var repositoryMock = new Mock<IClinicaRepository>();
        var mapperMock = new Mock<IMapper>();

        var clinica = new Clinica { IdClinica = 1 };

        var dto = new UpdateClinicaDto
        {
            Nome = "Clínica Pet Atualizada",
            Endereco = "Avenida Central, 200",
            Telefone = "11888888888"
        };

        repositoryMock
            .Setup(r => r.GetByIdAsync(1))
            .ReturnsAsync(clinica);

        repositoryMock
            .Setup(r => r.UpdateAsync(clinica))
            .Returns(Task.CompletedTask);

        var service = new ClinicaService(
            repositoryMock.Object,
            mapperMock.Object);

        await service.UpdateAsync(1, dto);

        mapperMock.Verify(
            m => m.Map(dto, clinica),
            Times.Once);

        repositoryMock.Verify(
            r => r.UpdateAsync(clinica),
            Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_ClinicaInexistente_LancaNotFoundException()
    {
        var repositoryMock = new Mock<IClinicaRepository>();
        var mapperMock = new Mock<IMapper>();

        repositoryMock
            .Setup(r => r.GetByIdAsync(999))
            .ReturnsAsync((Clinica?)null);

        var service = new ClinicaService(
            repositoryMock.Object,
            mapperMock.Object);

        await Assert.ThrowsAsync<NotFoundException>(
            () => service.UpdateAsync(999, new UpdateClinicaDto()));
    }

    [Fact]
    public async Task DeleteAsync_IdValido_ChamaRepositorio()
    {
        var repositoryMock = new Mock<IClinicaRepository>();
        var mapperMock = new Mock<IMapper>();

        repositoryMock
            .Setup(r => r.DeleteAsync(1))
            .Returns(Task.CompletedTask);

        var service = new ClinicaService(
            repositoryMock.Object,
            mapperMock.Object);

        await service.DeleteAsync(1);

        repositoryMock.Verify(
            r => r.DeleteAsync(1),
            Times.Once);
    }
}
