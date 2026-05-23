using System.Text.Json;
using PetCare.Application.Exceptions;

namespace PetCare.API.Middlewares;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;

    public ExceptionMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }

        catch (NotFoundException ex)
        {
            context.Response.ContentType = "application/json";

            context.Response.StatusCode = StatusCodes.Status404NotFound;

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
            context.Response.ContentType = "application/json";

            context.Response.StatusCode = StatusCodes.Status500InternalServerError;

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