namespace WebApi.Presentation;

public static class ExceptionHandlingConfiguration
{
    public static void ConfigureExceptionHandling(this WebApplication app) =>
        app.Use(async (context, next) =>
        {
            try
            {
                await next();
            }
            catch (BadHttpRequestException ex)
            {
                app.Logger.LogWarning(ex, "Bad request error.");
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                await context.Response.WriteAsync("Bad Request: Invalid input.");
            }
            catch (Exception ex)
            {
                app.Logger.LogError(ex, "Internal server error.");
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                await context.Response.WriteAsync("Internal Server Error. Please try again later.");
            }
        });
}