using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;
using var spaClient =
    SpaProxy.CreateHttpClientForProxy(configuration.GetSection("NodeServerTimeout").Get<TimeSpan>());
var spaUrl = new UriBuilder(configuration["NodeServerUrl"]!).Uri;

var services = builder.Services;
services.AddControllers();

var app = builder.Build();

app.UseRouting();

app.MapGet("/hello", () => "hello world").WithDisplayName("hello");
app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
app.Use(ServeNodeAsync);
app.Run();


async Task ServeNodeAsync(HttpContext context, Func<Task> next)
{
    var logger = context.RequestServices.GetRequiredService<ILogger<SpaProxy>>();
    if (context.GetEndpoint() is { } endpoint)
    {
        logger.LogInformation("Stop node proxy serving endpoint found {Endpoint}", endpoint);
        await next();
        return;
    }

    var applicationStopping =
        context.RequestServices.GetRequiredService<IHostApplicationLifetime>().ApplicationStopping;
    var path = context.Request.Path;
    logger.LogInformation("Serving node proxy {Path}", path);
    var result = await SpaProxy.PerformProxyRequestAsync(context,
        spaClient,
        spaUrl,
        applicationStopping,
        true);
    if (!result)
    {
        logger.LogError("Serving node proxy failed {Path}", path);
        await next();
    }
    else
    {
        logger.LogInformation("Serving node proxy success {Path}", path);
    }
}

public record SignInModel(string Login, string Password);