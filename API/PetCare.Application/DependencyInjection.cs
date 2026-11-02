using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using PetCare.Application.Interfaces;
using PetCare.Application.Mappings;
using PetCare.Application.Services;

namespace PetCare.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(
        this IServiceCollection services)
    {
        services.AddAutoMapper(cfg => { }, typeof(TutorProfile));

        services.AddScoped<
            IAplicacaoVacinaService,
            AplicacaoVacinaService>();

        services.AddScoped<
            IHistoricoSaudeService,
            HistoricoSaudeService>();

        services.AddScoped<
            IConsultaService,
            ConsultaService>();

        services.AddScoped<
            IClinicaService,
            ClinicaService>();

        services.AddScoped<
            IVacinaService,
            VacinaService>();

        services.AddScoped<
            IPetService,
            PetService>();

        services.AddScoped<
            ITutorService,
            TutorService>();

        return services;
    }
}