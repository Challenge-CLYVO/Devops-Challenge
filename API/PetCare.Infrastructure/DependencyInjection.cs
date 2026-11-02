using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PetCare.Application.Interfaces;
using PetCare.Infrastructure.Data;
using PetCare.Infrastructure.Repositories;

namespace PetCare.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var connectionString =
            configuration.GetConnectionString(
                "RecommendaContextOracle");

        if (string.IsNullOrWhiteSpace(connectionString))
        {
            throw new InvalidOperationException(
                "A connection string 'RecommendaContextOracle' " +
                "não foi configurada.");
        }

        services.AddDbContext<AppDbContext>(options =>
            options.UseOracle(connectionString));

        services.AddScoped<
            IAplicacaoVacinaRepository,
            AplicacaoVacinaRepository>();

        services.AddScoped<
            IHistoricoSaudeRepository,
            HistoricoSaudeRepository>();

        services.AddScoped<
            IConsultaRepository,
            ConsultaRepository>();

        services.AddScoped<
            IClinicaRepository,
            ClinicaRepository>();

        services.AddScoped<
            IVacinaRepository,
            VacinaRepository>();

        services.AddScoped<
            IPetRepository,
            PetRepository>();

        services.AddScoped<
            ITutorRepository,
            TutorRepository>();

        return services;
    }
}