using System.Text.Json;
using PetCare.Application.Exceptions;
using PetCare.API.Metrics;

namespace PetCare.API.Middlewares;

public class ExceptionMiddleware
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
                "Recurso não encontrado. Path: {Path}",
                context.Request.Path);

            context.Response.ContentType = "application/json";

            context.Response.StatusCode =
                StatusCodes.Status404NotFound;

            var response = new
            {
                status = 404,
                mensagem = ex.Message
            };

            await context.Response.WriteAsync(
                JsonSerializer.Serialize(response)
            );
        }

        catch (Exception ex)
        {
            PetCareMetrics.HttpErrors.Add(1);

            _logger.LogError(
                ex,
                "Erro interno não tratado. Path: {Path}",
                context.Request.Path);

            context.Response.ContentType = "application/json";

            context.Response.StatusCode =
                StatusCodes.Status500InternalServerError;

            var response = new
            {
                status = 500,
                mensagem = "Erro interno no servidor.",
                detalhe = ex.Message
            };

            await context.Response.WriteAsync(
                JsonSerializer.Serialize(response)
            );
        }
    }
}