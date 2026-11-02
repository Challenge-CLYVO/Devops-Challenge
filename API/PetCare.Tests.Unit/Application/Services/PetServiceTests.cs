using AutoMapper;
using Microsoft.Extensions.Logging;
using Moq;
using PetCare.Application.DTOs.Pet;
using PetCare.Application.Exceptions;
using PetCare.Application.Interfaces;
using PetCare.Application.Services;
using PetCare.Domain.Entities;

namespace PetCare.Tests.Unit.Application.Services;

public class PetServiceTests
{
    [Fact]
    public async Task GetAllAsync_ComPets_RetornaListaDeDtos()
    {
        var repositoryMock = new Mock<IPetRepository>();
        var mapperMock = new Mock<IMapper>();

        var pets = new List<Pet>
        {
            new() { IdPet = 1, Nome = "Rex" },
            new() { IdPet = 2, Nome = "Luna" }
        };

        var dtos = new List<ReadPetDto>
        {
            new() { IdPet = 1, Nome = "Rex" },
            new() { IdPet = 2, Nome = "Luna" }
        };

        repositoryMock
            .Setup(r => r.GetAllAsync())
            .ReturnsAsync(pets);

        mapperMock
            .Setup(m => m.Map<IEnumerable<ReadPetDto>>(pets))
            .Returns(dtos);

        var service = new PetService(
            repositoryMock.Object,
            mapperMock.Object,
            Mock.Of<ILogger<PetService>>());

        var resultado = await service.GetAllAsync();

        Assert.NotNull(resultado);
        Assert.Equal(2, resultado.Count());
        Assert.Equal("Rex", resultado.First().Nome);

        repositoryMock.Verify(
            r => r.GetAllAsync(),
            Times.Once);
    }

    [Fact]
    public async Task GetByIdAsync_PetExistente_RetornaPet()
    {
        var repositoryMock = new Mock<IPetRepository>();
        var mapperMock = new Mock<IMapper>();

        var pet = new Pet { IdPet = 1, Nome = "Rex" };
        var dto = new ReadPetDto { IdPet = 1, Nome = "Rex" };

        repositoryMock
            .Setup(r => r.GetByIdAsync(1))
            .ReturnsAsync(pet);

        mapperMock
            .Setup(m => m.Map<ReadPetDto>(pet))
            .Returns(dto);

        var service = new PetService(
            repositoryMock.Object,
            mapperMock.Object,
            Mock.Of<ILogger<PetService>>());

        var resultado = await service.GetByIdAsync(1);

        Assert.NotNull(resultado);
        Assert.Equal(1, resultado!.IdPet);
        Assert.Equal("Rex", resultado.Nome);
    }

    [Fact]
    public async Task GetByIdAsync_PetNaoExistente_LancaNotFoundException()
    {
        var repositoryMock = new Mock<IPetRepository>();
        var mapperMock = new Mock<IMapper>();

        repositoryMock
            .Setup(r => r.GetByIdAsync(999))
            .ReturnsAsync((Pet?)null);

        var service = new PetService(
            repositoryMock.Object,
            mapperMock.Object,
            Mock.Of<ILogger<PetService>>());

        var act = () => service.GetByIdAsync(999);

        await Assert.ThrowsAsync<NotFoundException>(act);
    }

    [Fact]
    public async Task CreateAsync_DtoValido_AdicionaPetNoRepositorio()
    {
        var repositoryMock = new Mock<IPetRepository>();
        var mapperMock = new Mock<IMapper>();

        var dto = new CreatePetDto();
        var pet = new Pet { IdPet = 1 };

        mapperMock
            .Setup(m => m.Map<Pet>(dto))
            .Returns(pet);

        repositoryMock
            .Setup(r => r.AddAsync(pet))
            .Returns(Task.CompletedTask);

        var service = new PetService(
            repositoryMock.Object,
            mapperMock.Object,
            Mock.Of<ILogger<PetService>>());

        await service.CreateAsync(dto);

        mapperMock.Verify(
            m => m.Map<Pet>(dto),
            Times.Once);

        repositoryMock.Verify(
            r => r.AddAsync(pet),
            Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_PetExistente_AtualizaPet()
    {
        var repositoryMock = new Mock<IPetRepository>();
        var mapperMock = new Mock<IMapper>();

        var pet = new Pet { IdPet = 1 };
        var dto = new UpdatePetDto();

        repositoryMock
            .Setup(r => r.GetByIdAsync(1))
            .ReturnsAsync(pet);

        repositoryMock
            .Setup(r => r.UpdateAsync(pet))
            .Returns(Task.CompletedTask);

        var service = new PetService(
            repositoryMock.Object,
            mapperMock.Object,
            Mock.Of<ILogger<PetService>>());

        await service.UpdateAsync(1, dto);

        mapperMock.Verify(
            m => m.Map(dto, pet),
            Times.Once);

        repositoryMock.Verify(
            r => r.UpdateAsync(pet),
            Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_PetNaoExistente_LancaNotFoundException()
    {
        var repositoryMock = new Mock<IPetRepository>();
        var mapperMock = new Mock<IMapper>();

        repositoryMock
            .Setup(r => r.GetByIdAsync(999))
            .ReturnsAsync((Pet?)null);

        var service = new PetService(
            repositoryMock.Object,
            mapperMock.Object,
            Mock.Of<ILogger<PetService>>());

        var act = () => service.UpdateAsync(
            999,
            new UpdatePetDto());

        await Assert.ThrowsAsync<NotFoundException>(act);

        repositoryMock.Verify(
            r => r.UpdateAsync(It.IsAny<Pet>()),
            Times.Never);
    }

    [Fact]
    public async Task DeleteAsync_IdValido_ChamaRepositorio()
    {
        var repositoryMock = new Mock<IPetRepository>();
        var mapperMock = new Mock<IMapper>();

        repositoryMock
            .Setup(r => r.DeleteAsync(21))
            .Returns(Task.CompletedTask);

        var service = new PetService(
            repositoryMock.Object,
            mapperMock.Object,
            Mock.Of<ILogger<PetService>>());

        await service.DeleteAsync(21);

        repositoryMock.Verify(
            r => r.DeleteAsync(21),
            Times.Once);
    }
}
