using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace PetCare.API.HealthChecks;

public class ExternalServiceHealthCheck : IHealthCheck
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IConfiguration _configuration;

    public ExternalServiceHealthCheck(
        IHttpClientFactory httpClientFactory,
        IConfiguration configuration)
    {
        _httpClientFactory = httpClientFactory;
        _configuration = configuration;
    }

    public async Task<HealthCheckResult> CheckHealthAsync(
        HealthCheckContext context,
        CancellationToken cancellationToken = default)
    {
        var url = _configuration["HealthChecks:ExternalServiceUrl"];

        if (string.IsNullOrWhiteSpace(url))
        {
            return HealthCheckResult.Unhealthy(
                "URL do serviço externo não configurada.");
        }

        try
        {
            var client = _httpClientFactory.CreateClient();

            client.Timeout = TimeSpan.FromSeconds(5);

            var response = await client.GetAsync(
                url,
                cancellationToken);

            if (response.IsSuccessStatusCode)
            {
                return HealthCheckResult.Healthy(
                    "Serviço externo está disponível.");
            }

            return HealthCheckResult.Unhealthy(
                $"Serviço externo retornou HTTP {(int)response.StatusCode}.");
        }
        catch (Exception ex)
        {
            return HealthCheckResult.Unhealthy(
                "Serviço externo está indisponível.",
                ex);
        }
    }
}