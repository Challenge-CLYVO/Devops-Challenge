using System.Diagnostics.Metrics;

namespace PetCare.API.Metrics;

public static class PetCareMetrics
{
    public const string MeterName = "PetCare.API.Metrics";

    private static readonly Meter Meter =
        new(MeterName, "1.0.0");

    public static readonly Counter<long> HttpErrors =
        Meter.CreateCounter<long>(
            "petcare_http_errors_total",
            description: "Quantidade total de erros HTTP da aplicação.");
}