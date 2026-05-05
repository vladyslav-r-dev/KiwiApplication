using Microsoft.AspNetCore.Diagnostics;

namespace KiwiApp.Api.ErrorHandling;

public static class ExceptionHandlerExtensions
{
    public static void UseGlobalExceptionHandler(this WebApplication app)
    {
        app.UseExceptionHandler(exceptionApp =>
        {
            exceptionApp.Run(async context =>
            {
                var exceptionFeature = context.Features.Get<IExceptionHandlerPathFeature>();
                var exception = exceptionFeature?.Error;

                var error = ExceptionMapper.Map(
                    exception ?? new Exception("Unknown exception"),
                    context.Request.Path.Value);

                context.Response.StatusCode = error.StatusCode;
                context.Response.ContentType = "application/json";

                await context.Response.WriteAsJsonAsync(error);
            });
        });
    }
}