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
    public async Task GetByIdAsync_PetExistente_RetornaPet()
    {
        // Arrange
        var repositoryMock = new Mock<IPetRepository>();
        var mapperMock = new Mock<IMapper>();

        var pet = new Pet
        {
            IdPet = 1
        };

        var dto = new ReadPetDto
        {
            IdPet = 1
        };

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

        // Act
        var result = await service.GetByIdAsync(1);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(1, result!.IdPet);
    }
    
    [Fact]
    public async Task GetByIdAsync_PetNaoExistente_LancaNotFoundException()
    {
        // Arrange
        var repositoryMock = new Mock<IPetRepository>();
        var mapperMock = new Mock<IMapper>();

        repositoryMock
            .Setup(r => r.GetByIdAsync(999))
            .ReturnsAsync((Pet?)null);

        var service = new PetService(
            repositoryMock.Object,
            mapperMock.Object,
            Mock.Of<ILogger<PetService>>());

        // Act
        var act = () => service.GetByIdAsync(999);

        // Assert
        await Assert.ThrowsAsync<NotFoundException>(act);
    }
    
    [Fact]
    public async Task CreateAsync_DtoValido_AdicionaPetNoRepositorio()
    {
        // Arrange
        var repositoryMock = new Mock<IPetRepository>();
        var mapperMock = new Mock<IMapper>();

        var dto = new CreatePetDto();

        var pet = new Pet
        {
            IdPet = 1
        };

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

        // Act
        await service.CreateAsync(dto);

        // Assert
        repositoryMock.Verify(
            r => r.AddAsync(pet),
            Times.Once);
    }
    
    [Fact]
    public async Task UpdateAsync_PetExistente_AtualizaPet()
    {
        // Arrange
        var repositoryMock = new Mock<IPetRepository>();
        var mapperMock = new Mock<IMapper>();

        var pet = new Pet
        {
            IdPet = 1
        };

        var dto = new UpdatePetDto();

        repositoryMock
            .Setup(r => r.GetByIdAsync(1))
            .ReturnsAsync(pet);

        mapperMock
            .Setup(m => m.Map(dto, pet));

        repositoryMock
            .Setup(r => r.UpdateAsync(pet))
            .Returns(Task.CompletedTask);

        var service = new PetService(
            repositoryMock.Object,
            mapperMock.Object,
            Mock.Of<ILogger<PetService>>());

        // Act
        await service.UpdateAsync(1, dto);

        // Assert
        repositoryMock.Verify(
            r => r.UpdateAsync(pet),
            Times.Once);
    }
    
    [Fact]
    public async Task UpdateAsync_PetNaoExistente_LancaNotFoundException()
    {
        // Arrange
        var repositoryMock = new Mock<IPetRepository>();
        var mapperMock = new Mock<IMapper>();

        repositoryMock
            .Setup(r => r.GetByIdAsync(999))
            .ReturnsAsync((Pet?)null);

        var service = new PetService(
            repositoryMock.Object,
            mapperMock.Object,
            Mock.Of<ILogger<PetService>>());

        // Act
        var act = () => service.UpdateAsync(
            999,
            new UpdatePetDto());

        // Assert
        await Assert.ThrowsAsync<NotFoundException>(act);
    }
    
    [Fact]
    public async Task DeleteAsync_IdValido_ChamaRepositorio()
    {
        // Arrange
        var repositoryMock = new Mock<IPetRepository>();
        var mapperMock = new Mock<IMapper>();

        repositoryMock
            .Setup(r => r.DeleteAsync(1))
            .Returns(Task.CompletedTask);

        var service = new PetService(
            repositoryMock.Object,
            mapperMock.Object,
            Mock.Of<ILogger<PetService>>());

        // Act
        await service.DeleteAsync(21);

        // Assert
        repositoryMock.Verify(
            r => r.DeleteAsync(21),
            Times.Once);
    }
}