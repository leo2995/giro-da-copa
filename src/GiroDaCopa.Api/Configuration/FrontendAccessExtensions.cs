using GiroDaCopa.Api.Middleware;

namespace GiroDaCopa.Api.Configuration;

public static class FrontendAccessExtensions
{
    public static IServiceCollection AddGiroDaCopaFrontendAccess(
        this IServiceCollection services,
        IConfiguration configuration,
        IWebHostEnvironment environment)
    {
        services.Configure<FrontendAccessSettings>(options =>
        {
            options.ApiKey = configuration[FrontendAccessSettings.ApiKeyEnvName]
                ?? configuration[$"{FrontendAccessSettings.SectionName}:ApiKey"];
        });

        services.AddGiroDaCopaCors(configuration, environment);

        return services;
    }

    public static WebApplication UseGiroDaCopaFrontendAccess(this WebApplication app)
    {
        if (app.Environment.IsProduction())
        {
            var apiKey = app.Configuration[FrontendAccessSettings.ApiKeyEnvName]
                ?? app.Configuration[$"{FrontendAccessSettings.SectionName}:ApiKey"];
            var frontendUrl = app.Configuration["FRONTEND_URL"];
            var logger = app.Services
                .GetRequiredService<ILoggerFactory>()
                .CreateLogger("FrontendAccess");

            if (string.IsNullOrWhiteSpace(apiKey))
            {
                logger.LogCritical(
                    "{EnvVar} não configurado. A API ficará indisponível até definir a variável em Render → Environment.",
                    FrontendAccessSettings.ApiKeyEnvName);
            }

            if (string.IsNullOrWhiteSpace(frontendUrl))
            {
                logger.LogCritical(
                    "FRONTEND_URL não configurado. Defina a URL do Netlify em Render → Environment.");
            }
        }

        app.UseMiddleware<FrontendAccessMiddleware>();
        return app;
    }

    private static IServiceCollection AddGiroDaCopaCors(
        this IServiceCollection services,
        IConfiguration configuration,
        IWebHostEnvironment environment)
    {
        var frontendUrl = configuration["FRONTEND_URL"]?.TrimEnd('/');
        var allowedOrigins = string.IsNullOrWhiteSpace(frontendUrl)
            ? Array.Empty<string>()
            : [frontendUrl];

        if (environment.IsDevelopment() && allowedOrigins.Length == 0)
        {
            allowedOrigins =
            [
                "http://localhost:3000",
                "http://127.0.0.1:3000"
            ];
        }

        services.AddCors(options =>
        {
            options.AddDefaultPolicy(policy =>
            {
                if (allowedOrigins.Length > 0)
                {
                    policy.WithOrigins(allowedOrigins)
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                }
            });
        });

        return services;
    }
}
