using System.Security.Cryptography;
using System.Text;
using GiroDaCopa.Api.Configuration;
using Microsoft.Extensions.Options;

namespace GiroDaCopa.Api.Middleware;

public sealed class FrontendAccessMiddleware
{
    private readonly RequestDelegate _next;
    private readonly FrontendAccessSettings _settings;
    private readonly IHostEnvironment _environment;

    public FrontendAccessMiddleware(
        RequestDelegate next,
        IOptions<FrontendAccessSettings> settings,
        IHostEnvironment environment)
    {
        _next = next;
        _settings = settings.Value;
        _environment = environment;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        if (ShouldBypass(context))
        {
            await _next(context);
            return;
        }

        var apiKey = _settings.ApiKey;
        if (string.IsNullOrWhiteSpace(apiKey))
        {
            await _next(context);
            return;
        }

        if (!context.Request.Headers.TryGetValue(
                FrontendAccessSettings.ApiKeyHeaderName,
                out var provided)
            || !IsValidApiKey(provided.ToString(), apiKey))
        {
            context.Response.StatusCode = StatusCodes.Status403Forbidden;
            await context.Response.WriteAsync("Forbidden");
            return;
        }

        await _next(context);
    }

    private bool ShouldBypass(HttpContext context)
    {
        if (HttpMethods.IsOptions(context.Request.Method))
        {
            return true;
        }

        var path = context.Request.Path.Value ?? string.Empty;
        if (path.StartsWith("/health", StringComparison.OrdinalIgnoreCase))
        {
            return true;
        }

        if (_environment.IsDevelopment()
            && (path.StartsWith("/swagger", StringComparison.OrdinalIgnoreCase)
                || path.Equals("/", StringComparison.OrdinalIgnoreCase)))
        {
            return true;
        }

        return false;
    }

    private static bool IsValidApiKey(string provided, string expected)
    {
        if (string.IsNullOrEmpty(provided) || string.IsNullOrEmpty(expected))
        {
            return false;
        }

        var providedBytes = Encoding.UTF8.GetBytes(provided);
        var expectedBytes = Encoding.UTF8.GetBytes(expected);

        return providedBytes.Length == expectedBytes.Length
            && CryptographicOperations.FixedTimeEquals(providedBytes, expectedBytes);
    }
}
