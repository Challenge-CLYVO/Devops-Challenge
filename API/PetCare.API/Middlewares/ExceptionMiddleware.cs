using Microsoft.AspNetCore.Mvc;
using PetCare.API.Metrics;
using PetCare.Application.Exceptions;

namespace PetCare.API.Middlewares;

public sealed class ExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionMiddleware> _logger;

    public ExceptionMiddleware(
        RequestDelegate next,
        ILogger<ExceptionMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (NotFoundException ex)
        {
            PetCareMetrics.HttpErrors.Add(1);

            _logger.LogWarning(
                ex,
                "Recurso não encontrado. Path: {Path}; TraceId: {TraceId}",
                context.Request.Path,
                context.TraceIdentifier);

            await WriteProblemDetailsAsync(
                context,
                StatusCodes.Status404NotFound,
                "Recurso não encontrado",
                ex.Message);
        }
        catch (ArgumentException ex)
        {
            PetCareMetrics.HttpErrors.Add(1);

            _logger.LogWarning(
                ex,
                "Requisição inválida. Path: {Path}; TraceId: {TraceId}",
                context.Request.Path,
                context.TraceIdentifier);

            await WriteProblemDetailsAsync(
                context,
                StatusCodes.Status400BadRequest,
                "Requisição inválida",
                "Um ou mais valores informados são inválidos.");
        }
        catch (Exception ex)
        {
            PetCareMetrics.HttpErrors.Add(1);

            _logger.LogError(
                ex,
                "Erro interno não tratado. Path: {Path}; TraceId: {TraceId}",
                context.Request.Path,
                context.TraceIdentifier);

            await WriteProblemDetailsAsync(
                context,
                StatusCodes.Status500InternalServerError,
                "Erro interno no servidor",
                "Ocorreu um erro inesperado. Tente novamente mais tarde.");
        }
    }

    private static async Task WriteProblemDetailsAsync(
        HttpContext context,
        int statusCode,
        string title,
        string detail)
    {
        if (context.Response.HasStarted)
        {
            throw new InvalidOperationException(
                "Não é possível alterar a resposta HTTP porque ela já foi iniciada.");
        }

        context.Response.Clear();
        context.Response.StatusCode = statusCode;
        context.Response.ContentType = "application/problem+json";

        var problem = new ProblemDetails
        {
            Status = statusCode,
            Title = title,
            Detail = detail,
            Instance = context.Request.Path
        };

        problem.Extensions["traceId"] = context.TraceIdentifier;

        if (context.Response.Headers.TryGetValue(
                "X-Correlation-ID",
                out var correlationId)
            && !string.IsNullOrWhiteSpace(correlationId))
        {
            problem.Extensions["correlationId"] = correlationId.ToString();
        }

        await context.Response.WriteAsJsonAsync(
            problem,
            cancellationToken: context.RequestAborted);
    }
}