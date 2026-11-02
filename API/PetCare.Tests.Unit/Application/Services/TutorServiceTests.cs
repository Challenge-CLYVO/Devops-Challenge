using AutoMapper;
using Moq;
using PetCare.Application.DTOs.Tutor;
using PetCare.Application.Exceptions;
using PetCare.Application.Interfaces;
using PetCare.Application.Services;
using PetCare.Domain.Entities;

namespace PetCare.Tests.Unit.Application.Services;

public class TutorServiceTests
{
    [Fact]
    public async Task GetAllAsync_ExistemTutores_RetornaListaDeDtos()
    {
        var repositoryMock = new Mock<ITutorRepository>();
        var mapperMock = new Mock<IMapper>();

        var tutores = new List<Tutor>
        {
            new() { IdTutor = 1 },
            new() { IdTutor = 2 }
        };

        var dtos = new List<ReadTutorDto>
        {
            new() { IdTutor = 1 },
            new() { IdTutor = 2 }
        };

        repositoryMock
            .Setup(r => r.GetAllAsync())
            .ReturnsAsync(tutores);

        mapperMock
            .Setup(m => m.Map<IEnumerable<ReadTutorDto>>(tutores))
            .Returns(dtos);

        var service = new TutorService(
            repositoryMock.Object,
            mapperMock.Object);

        var resultado = await service.GetAllAsync();

        Assert.NotNull(resultado);
        Assert.Equal(2, resultado.Count());
        repositoryMock.Verify(r => r.GetAllAsync(), Times.Once);
    }

    [Fact]
    public async Task GetByIdAsync_TutorExistente_RetornaDto()
    {
        var repositoryMock = new Mock<ITutorRepository>();
        var mapperMock = new Mock<IMapper>();

        var tutor = new Tutor { IdTutor = 1 };
        var dto = new ReadTutorDto { IdTutor = 1 };

        repositoryMock
            .Setup(r => r.GetByIdAsync(1))
            .ReturnsAsync(tutor);

        mapperMock
            .Setup(m => m.Map<ReadTutorDto>(tutor))
            .Returns(dto);

        var service = new TutorService(
            repositoryMock.Object,
            mapperMock.Object);

        var resultado = await service.GetByIdAsync(1);

        Assert.NotNull(resultado);
        Assert.Equal(1, resultado!.IdTutor);
    }

    [Fact]
    public async Task GetByIdAsync_TutorInexistente_LancaNotFoundException()
    {
        var repositoryMock = new Mock<ITutorRepository>();
        var mapperMock = new Mock<IMapper>();

        repositoryMock
            .Setup(r => r.GetByIdAsync(999))
            .ReturnsAsync((Tutor?)null);

        var service = new TutorService(
            repositoryMock.Object,
            mapperMock.Object);

        await Assert.ThrowsAsync<NotFoundException>(
            () => service.GetByIdAsync(999));
    }

    [Fact]
    public async Task CreateAsync_DtoValido_AdicionaTutorNoRepositorio()
    {
        var repositoryMock = new Mock<ITutorRepository>();
        var mapperMock = new Mock<IMapper>();

        var dto = new CreateTutorDto
        {
            Nome = "Maria Silva",
            Telefone = "11999999999",
            Email = "maria@email.com"
        };

        var tutor = new Tutor { IdTutor = 1 };

        mapperMock
            .Setup(m => m.Map<Tutor>(dto))
            .Returns(tutor);

        repositoryMock
            .Setup(r => r.AddAsync(tutor))
            .Returns(Task.CompletedTask);

        var service = new TutorService(
            repositoryMock.Object,
            mapperMock.Object);

        await service.CreateAsync(dto);

        repositoryMock.Verify(
            r => r.AddAsync(tutor),
            Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_TutorExistente_AtualizaTutor()
    {
        var repositoryMock = new Mock<ITutorRepository>();
        var mapperMock = new Mock<IMapper>();

        var tutor = new Tutor { IdTutor = 1 };

        var dto = new UpdateTutorDto
        {
            Nome = "Maria Silva",
            Telefone = "11999999999",
            Email = "maria@email.com"
        };

        repositoryMock
            .Setup(r => r.GetByIdAsync(1))
            .ReturnsAsync(tutor);

        repositoryMock
            .Setup(r => r.UpdateAsync(tutor))
            .Returns(Task.CompletedTask);

        var service = new TutorService(
            repositoryMock.Object,
            mapperMock.Object);

        await service.UpdateAsync(1, dto);

        mapperMock.Verify(
            m => m.Map(dto, tutor),
            Times.Once);

        repositoryMock.Verify(
            r => r.UpdateAsync(tutor),
            Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_TutorInexistente_LancaNotFoundException()
    {
        var repositoryMock = new Mock<ITutorRepository>();
        var mapperMock = new Mock<IMapper>();

        repositoryMock
            .Setup(r => r.GetByIdAsync(999))
            .ReturnsAsync((Tutor?)null);

        var service = new TutorService(
            repositoryMock.Object,
            mapperMock.Object);

        await Assert.ThrowsAsync<NotFoundException>(
            () => service.UpdateAsync(999, new UpdateTutorDto()));
    }

    [Fact]
    public async Task DeleteAsync_IdValido_ChamaRepositorio()
    {
        var repositoryMock = new Mock<ITutorRepository>();
        var mapperMock = new Mock<IMapper>();

        repositoryMock
            .Setup(r => r.DeleteAsync(1))
            .Returns(Task.CompletedTask);

        var service = new TutorService(
            repositoryMock.Object,
            mapperMock.Object);

        await service.DeleteAsync(1);

        repositoryMock.Verify(
            r => r.DeleteAsync(1),
            Times.Once);
    }
}
