using System.Reflection;
using System.Diagnostics.Metrics;
using System.Text;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

using Serilog;
using Serilog.Events;

using PetCare.API.HealthChecks;
using PetCare.API.Middlewares;
using PetCare.API.Metrics;

using PetCare.Application.Interfaces;
using PetCare.Application.Services;
using PetCare.Application.Mappings;

using PetCare.Infrastructure.Data;
using PetCare.Infrastructure.Repositories;

// =====================================================
// SERILOG
// =====================================================

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
    .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Warning)
    .MinimumLevel.Override(
        "Microsoft.Hosting.Lifetime",
        LogEventLevel.Information)
    .Enrich.FromLogContext()
    .Enrich.WithProperty("Application", "PetCare.API")
    .WriteTo.Console()
    .WriteTo.File(
        "logs/petcare-.log",
        rollingInterval: RollingInterval.Day,
        retainedFileCountLimit: 7)
    .CreateLogger();

try
{
    var builder = WebApplication.CreateBuilder(args);

    const string serviceName = "PetCare.API";
    const string serviceVersion = "1.0.0";

    builder.Host.UseSerilog();

    // =================================================
    // OPEN TELEMETRY
    // =================================================

    var meter = new Meter(
        "PetCare.API.Metrics",
        serviceVersion);

    meter.CreateCounter<long>(
        "petcare_http_errors_total",
        description: "Quantidade total de erros HTTP da aplicação.");

    builder.Services
        .AddOpenTelemetry()
        .WithTracing(tracing =>
        {
            tracing
                .SetResourceBuilder(
                    ResourceBuilder.CreateDefault()
                        .AddService(
                            serviceName,
                            serviceVersion: serviceVersion))
                .AddAspNetCoreInstrumentation()
                .AddHttpClientInstrumentation()
                .AddEntityFrameworkCoreInstrumentation()
                .AddConsoleExporter();
        })
        .WithMetrics(metrics =>
        {
            metrics
                .SetResourceBuilder(
                    ResourceBuilder.CreateDefault()
                        .AddService(
                            serviceName,
                            serviceVersion: serviceVersion))
                .AddMeter(PetCareMetrics.MeterName)
                .AddMeter("PetCare.API.Metrics")
                .AddAspNetCoreInstrumentation()
                .AddHttpClientInstrumentation()
                .AddRuntimeInstrumentation()
                .AddPrometheusExporter();
        });

    // =================================================
    // BANCO ORACLE
    // =================================================

    var connectionString =
        builder.Configuration.GetConnectionString(
            "RecommendaContextOracle");

    if (string.IsNullOrWhiteSpace(connectionString))
    {
        throw new InvalidOperationException(
            "A connection string 'RecommendaContextOracle' " +
            "não foi configurada.");
    }

    builder.Services.AddDbContext<AppDbContext>(options =>
        options.UseOracle(connectionString));

    // =================================================
    // AUTOMAPPER
    // =================================================

    builder.Services.AddAutoMapper(
        cfg => { },
        typeof(TutorProfile));

    // =================================================
    // REPOSITORIES
    // =================================================

    builder.Services.AddScoped<
        IAplicacaoVacinaRepository,
        AplicacaoVacinaRepository>();

    builder.Services.AddScoped<
        IHistoricoSaudeRepository,
        HistoricoSaudeRepository>();

    builder.Services.AddScoped<
        IConsultaRepository,
        ConsultaRepository>();

    builder.Services.AddScoped<
        IClinicaRepository,
        ClinicaRepository>();

    builder.Services.AddScoped<
        IVacinaRepository,
        VacinaRepository>();

    builder.Services.AddScoped<
        IPetRepository,
        PetRepository>();

    builder.Services.AddScoped<
        ITutorRepository,
        TutorRepository>();

    // =================================================
    // SERVICES
    // =================================================

    builder.Services.AddScoped<
        IAplicacaoVacinaService,
        AplicacaoVacinaService>();

    builder.Services.AddScoped<
        IHistoricoSaudeService,
        HistoricoSaudeService>();

    builder.Services.AddScoped<
        IConsultaService,
        ConsultaService>();

    builder.Services.AddScoped<
        IClinicaService,
        ClinicaService>();

    builder.Services.AddScoped<
        IVacinaService,
        VacinaService>();

    builder.Services.AddScoped<
        IPetService,
        PetService>();

    builder.Services.AddScoped<
        ITutorService,
        TutorService>();

    // =================================================
    // JWT AUTHENTICATION
    // =================================================

    var jwtKey = builder.Configuration["Jwt:Key"];
    var jwtIssuer = builder.Configuration["Jwt:Issuer"];
    var jwtAudience = builder.Configuration["Jwt:Audience"];

    if (string.IsNullOrWhiteSpace(jwtKey))
    {
        throw new InvalidOperationException(
            "A configuração Jwt:Key não foi definida.");
    }

    if (Encoding.UTF8.GetByteCount(jwtKey) < 32)
    {
        throw new InvalidOperationException(
            "A chave JWT deve possuir pelo menos 32 bytes.");
    }

    if (string.IsNullOrWhiteSpace(jwtIssuer))
    {
        throw new InvalidOperationException(
            "A configuração Jwt:Issuer não foi definida.");
    }

    if (string.IsNullOrWhiteSpace(jwtAudience))
    {
        throw new InvalidOperationException(
            "A configuração Jwt:Audience não foi definida.");
    }

    builder.Services
        .AddAuthentication(
            JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters =
                new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,

                    ValidIssuer = jwtIssuer,
                    ValidAudience = jwtAudience,

                    IssuerSigningKey =
                        new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(jwtKey)),

                    ClockSkew = TimeSpan.FromMinutes(1)
                };
        });

    builder.Services.AddAuthorization();

    // =================================================
    // CONTROLLERS E HTTP CLIENT
    // =================================================

    builder.Services.AddControllers();
    builder.Services.AddHttpClient();

    // =================================================
    // HEALTH CHECKS
    // =================================================

    builder.Services
        .AddHealthChecks()
        .AddCheck<ApiHealthCheck>("api")
        .AddDbContextCheck<AppDbContext>(
            "oracle",
            tags: new[] { "db", "oracle" })
        .AddCheck<ExternalServiceHealthCheck>(
            "external-service");

    // =================================================
    // SWAGGER
    // =================================================

    builder.Services.AddEndpointsApiExplorer();

    builder.Services.AddSwaggerGen(options =>
    {
        var xmlFile =
            $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";

        var xmlPath = Path.Combine(
            AppContext.BaseDirectory,
            xmlFile);

        if (File.Exists(xmlPath))
        {
            options.IncludeXmlComments(xmlPath);
        }

        options.AddSecurityDefinition(
            "Bearer",
            new OpenApiSecurityScheme
            {
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                Scheme = "bearer",
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Description =
                    "Informe o token JWT no formato Bearer {seu_token}."
            });

        options.AddSecurityRequirement(
            new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    Array.Empty<string>()
                }
            });
    });

    // =================================================
    // PIPELINE HTTP
    // =================================================

    var app = builder.Build();

    app.UseSerilogRequestLogging();

    app.UseMiddleware<CorrelationIdMiddleware>();

    app.UseMiddleware<ExceptionMiddleware>();

    app.UseSwagger();
    app.UseSwaggerUI();

    app.UseAuthentication();
    app.UseAuthorization();

    app.MapControllers();

    // =================================================
    // PROMETHEUS METRICS
    // =================================================

    app.MapPrometheusScrapingEndpoint();

    // =================================================
    // HEALTH ENDPOINT
    // =================================================

    app.MapHealthChecks(
        "/health",
        new HealthCheckOptions
        {
            ResponseWriter = async (context, report) =>
            {
                context.Response.ContentType = "application/json";

                var response = new
                {
                    status = report.Status.ToString(),

                    checks = report.Entries.Select(entry => new
                    {
                        name = entry.Key,
                        status = entry.Value.Status.ToString(),
                        description = entry.Value.Description
                    }),

                    totalDuration = report.TotalDuration
                };

                await context.Response.WriteAsJsonAsync(response);
            }
        });

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(
        ex,
        "A aplicação PetCare.API foi encerrada inesperadamente.");
}
finally
{
    Log.CloseAndFlush();
}

// Necessário para WebApplicationFactory nos testes de integração.
public partial class Program
{
}