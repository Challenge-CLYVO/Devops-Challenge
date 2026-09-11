using System.Net;
using System.Net.Http.Json;
using System.Net.Http.Headers;
using PetCare.Application.DTOs.Pet;
using PetCare.Tests.Integration.Fixtures;

namespace PetCare.Tests.Integration.Controllers;

[Collection("PetCare Collection")]
public class PetControllerTests
{
    private readonly HttpClient _client;

    public PetControllerTests(PetCareApiFactory factory)
    {
        _client = factory.CreateClient();
    }

    private async Task AutenticarAsync()
    {
        var response = await _client.PostAsync(
            "/api/Auth/login",
            null);

        response.EnsureSuccessStatusCode();

        var result = await response.Content
            .ReadFromJsonAsync<LoginResponse>();

        Assert.NotNull(result);
        Assert.False(string.IsNullOrEmpty(result!.Token));

        _client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue(
                "Bearer",
                result.Token);
    }

    [Fact]
    public async Task GetAllAsync_RequisicaoValida_RetornaSucesso()
    {
        // Arrange
        await AutenticarAsync();

        // Act
        var response = await _client.GetAsync("/api/Pet");

        // Assert
        Assert.Equal(
            HttpStatusCode.OK,
            response.StatusCode);
    }

    [Fact]
    public async Task GetById_PetExistente_RetornaSucesso()
    {
        // Arrange
        await AutenticarAsync();

        var id = 1;

        // Act
        var response = await _client.GetAsync(
            $"/api/Pet/{id}");

        // Assert
        Assert.Equal(
            HttpStatusCode.OK,
            response.StatusCode);
    }

    [Fact]
    public async Task GetById_PetNaoExistente_RetornaNotFound()
    {
        // Arrange
        await AutenticarAsync();

        var id = 999999;

        // Act
        var response = await _client.GetAsync(
            $"/api/Pet/{id}");

        // Assert
        Assert.Equal(
            HttpStatusCode.NotFound,
            response.StatusCode);
    }

    [Fact]
    public async Task Create_PetValido_RetornaCreated()
    {
        // Arrange
        await AutenticarAsync();

        var dto = new CreatePetDto
        {
            Nome = $"Pet Teste Integracao {Guid.NewGuid()}",
            Idade = 3,
            Especie = "Cachorro",
            Raca = "Labrador",
            IdTutor = 1
        };

        // Act
        var response = await _client.PostAsJsonAsync(
            "/api/Pet",
            dto);

        // Assert
        Assert.Equal(
            HttpStatusCode.Created,
            response.StatusCode);
    }

    [Fact]
    public async Task Create_DadosInvalidos_RetornaBadRequest()
    {
        // Arrange
        await AutenticarAsync();

        var dto = new CreatePetDto
        {
            Nome = "",
            Idade = 100,
            Especie = "",
            Raca = "Teste",
            IdTutor = 1
        };

        // Act
        var response = await _client.PostAsJsonAsync(
            "/api/Pet",
            dto);

        // Assert
        Assert.Equal(
            HttpStatusCode.BadRequest,
            response.StatusCode);
    }

    [Fact]
    public async Task Update_PetExistente_RetornaNoContent()
    {
        // Arrange
        await AutenticarAsync();

        var dto = new UpdatePetDto
        {
            Nome = "Pet Atualizado Integracao",
            Idade = 4,
            Especie = "Cachorro",
            Raca = "Labrador",
            IdTutor = 1
        };

        // Act
        var response = await _client.PutAsJsonAsync(
            "/api/Pet/1",
            dto);

        // Assert
        Assert.Equal(
            HttpStatusCode.NoContent,
            response.StatusCode);
    }

    [Fact]
    public async Task Update_PetNaoExistente_RetornaNotFound()
    {
        // Arrange
        await AutenticarAsync();

        var id = 999999;

        var dto = new UpdatePetDto
        {
            Nome = "Pet Inexistente",
            Idade = 4,
            Especie = "Cachorro",
            Raca = "Labrador",
            IdTutor = 1
        };

        // Act
        var response = await _client.PutAsJsonAsync(
            $"/api/Pet/{id}",
            dto);

        // Assert
        Assert.Equal(
            HttpStatusCode.NotFound,
            response.StatusCode);
    }

    [Fact]
    public async Task Delete_PetExistente_RetornaNoContent()
    {
        // Arrange
        await AutenticarAsync();

        var id = 2;

        // Act
        var response = await _client.DeleteAsync(
            $"/api/Pet/{id}");

        // Assert
        Assert.Equal(
            HttpStatusCode.NoContent,
            response.StatusCode);
    }

    [Fact]
    public async Task GetAll_SemAutenticacao_RetornaUnauthorized()
    {
        // Arrange
        _client.DefaultRequestHeaders.Authorization = null;

        // Act
        var response = await _client.GetAsync(
            "/api/Pet");

        // Assert
        Assert.Equal(
            HttpStatusCode.Unauthorized,
            response.StatusCode);
    }

    [Fact]
    public async Task GetAll_ComTokenInvalido_RetornaUnauthorized()
    {
        // Arrange
        _client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue(
                "Bearer",
                "token-invalido");

        // Act
        var response = await _client.GetAsync(
            "/api/Pet");

        // Assert
        Assert.Equal(
            HttpStatusCode.Unauthorized,
            response.StatusCode);
    }

    [Fact]
    public async Task Pet_FluxoCompleto_CriarConsultarAtualizarExcluir()
    {
        // Arrange
        await AutenticarAsync();

        var nome = $"Pet Fluxo {Guid.NewGuid()}";

        var createDto = new CreatePetDto
        {
            Nome = nome,
            Idade = 3,
            Especie = "Cachorro",
            Raca = "Labrador",
            IdTutor = 1
        };

        // Act - CREATE
        var createResponse = await _client.PostAsJsonAsync(
            "/api/Pet",
            createDto);

        // Assert - CREATE
        Assert.Equal(
            HttpStatusCode.Created,
            createResponse.StatusCode);

        // Act - GET ALL
        var getAllResponse = await _client.GetAsync(
            "/api/Pet");

        // Assert - GET ALL
        Assert.Equal(
            HttpStatusCode.OK,
            getAllResponse.StatusCode);

        var pets = await getAllResponse.Content
            .ReadFromJsonAsync<List<ReadPetDto>>();

        Assert.NotNull(pets);

        var petCriado = pets!
            .FirstOrDefault(p => p.Nome == nome);

        Assert.NotNull(petCriado);

        var id = petCriado!.IdPet;

        // Act - GET BY ID
        var getResponse = await _client.GetAsync(
            $"/api/Pet/{id}");

        // Assert - GET BY ID
        Assert.Equal(
            HttpStatusCode.OK,
            getResponse.StatusCode);

        // Arrange - UPDATE
        var updateDto = new UpdatePetDto
        {
            Nome = $"{nome} Atualizado",
            Idade = 4,
            Especie = "Cachorro",
            Raca = "Golden Retriever",
            IdTutor = 1
        };

        // Act - UPDATE
        var updateResponse = await _client.PutAsJsonAsync(
            $"/api/Pet/{id}",
            updateDto);

        // Assert - UPDATE
        Assert.Equal(
            HttpStatusCode.NoContent,
            updateResponse.StatusCode);

        // Act - GET AFTER UPDATE
        var getUpdatedResponse = await _client.GetAsync(
            $"/api/Pet/{id}");

        // Assert - GET AFTER UPDATE
        Assert.Equal(
            HttpStatusCode.OK,
            getUpdatedResponse.StatusCode);

        var updatedPet = await getUpdatedResponse.Content
            .ReadFromJsonAsync<ReadPetDto>();

        Assert.NotNull(updatedPet);

        Assert.Equal(
            $"{nome} Atualizado",
            updatedPet!.Nome);

        // Act - DELETE
        var deleteResponse = await _client.DeleteAsync(
            $"/api/Pet/{id}");

        // Assert - DELETE
        Assert.Equal(
            HttpStatusCode.NoContent,
            deleteResponse.StatusCode);

        // Act - GET AFTER DELETE
        var getDeletedResponse = await _client.GetAsync(
            $"/api/Pet/{id}");

        // Assert - GET AFTER DELETE
        Assert.Equal(
            HttpStatusCode.NotFound,
            getDeletedResponse.StatusCode);
    }

    private class LoginResponse
    {
        public string Token { get; set; } = string.Empty;
    }
}

