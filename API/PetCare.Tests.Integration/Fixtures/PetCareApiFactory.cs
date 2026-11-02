using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using PetCare.Domain.Entities;
using PetCare.Infrastructure.Data;
using PetCare.Tests.Integration.Authentication;

namespace PetCare.Tests.Integration.Fixtures;

public class PetCareApiFactory : WebApplicationFactory<Program>
{ 
    private readonly string _databaseName =
        $"PetCareTests_{Guid.NewGuid():N}";

protected override void ConfigureWebHost(IWebHostBuilder builder)
{
    builder.ConfigureTestServices(services =>
    {
        // Remove a configuração do Oracle somente no ambiente de testes.
        services.RemoveAll<IDbContextOptionsConfiguration<AppDbContext>>();
        services.RemoveAll<DbContextOptions<AppDbContext>>();
        services.RemoveAll<AppDbContext>();

        // Cada factory utiliza seu próprio banco em memória.
        services.AddDbContext<AppDbContext>(options =>
            options.UseInMemoryDatabase(_databaseName));

        // Usa o autenticador de teste em vez de validar JWTs reais.
        services
            .AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme =
                    TestAuthHandler.SchemeName;

                options.DefaultChallengeScheme =
                    TestAuthHandler.SchemeName;

                options.DefaultForbidScheme =
                    TestAuthHandler.SchemeName;
            })
            .AddScheme<
                AuthenticationSchemeOptions,
                TestAuthHandler>(
                TestAuthHandler.SchemeName,
                _ => { });
    });
}

public void SeedDatabase()
{
    using var scope = Services.CreateScope();

    var context =
        scope.ServiceProvider.GetRequiredService<AppDbContext>();

    context.Database.EnsureCreated();

    if (context.Tutors.Any())
    {
        return;
    }

    var tutor = new Tutor
    {
        IdTutor = 1,
        Nome = "Tutor de Teste",
        Telefone = "(11) 99999-0000",
        Email = "tutor.teste@example.com"
    };

    context.Tutors.Add(tutor);

    context.Pets.AddRange(
        new Pet
        {
            IdPet = 1,
            Nome = "Rex Teste",
            Idade = 3,
            Especie = "Cachorro",
            Raca = "Labrador",
            IdTutor = tutor.IdTutor,
            Tutor = tutor
        },
        new Pet
        {
            IdPet = 2,
            Nome = "Mimi Teste",
            Idade = 2,
            Especie = "Gato",
            Raca = "Siamês",
            IdTutor = tutor.IdTutor,
            Tutor = tutor
        });

    context.SaveChanges();
}

}