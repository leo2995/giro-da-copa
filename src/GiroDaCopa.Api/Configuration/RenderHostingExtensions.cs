using System.Text.RegularExpressions;

namespace GiroDaCopa.Api.Configuration;

public static class RenderHostingExtensions
{
    public static WebApplicationBuilder ConfigureRenderHosting(this WebApplicationBuilder builder)
    {
        var port = Environment.GetEnvironmentVariable("PORT") ?? "8080";
        builder.WebHost.UseUrls($"http://+:{port}");

        var databaseUrl = builder.Configuration["DATABASE_URL"];
        var connectionString = builder.Configuration.GetConnectionString("Default");

        if (!string.IsNullOrWhiteSpace(databaseUrl))
        {
            builder.Configuration["ConnectionStrings:Default"] = ParsePostgresUrl(databaseUrl);
        }
        else if (LooksLikePostgresUrl(connectionString))
        {
            builder.Configuration["ConnectionStrings:Default"] = ParsePostgresUrl(connectionString!);
        }

        return builder;
    }

    private static bool LooksLikePostgresUrl(string? value) =>
        !string.IsNullOrWhiteSpace(value) &&
        Regex.IsMatch(value, @"^postgres(ql)?://", RegexOptions.IgnoreCase);

    private static string ParsePostgresUrl(string databaseUrl)
    {
        var uri = new Uri(databaseUrl);
        var userInfo = uri.UserInfo.Split(':', 2);
        var username = Uri.UnescapeDataString(userInfo[0]);
        var password = userInfo.Length > 1 ? Uri.UnescapeDataString(userInfo[1]) : string.Empty;
        var database = uri.AbsolutePath.TrimStart('/');

        return
            $"Host={uri.Host};" +
            $"Port={(uri.Port > 0 ? uri.Port : 5432)};" +
            $"Database={database};" +
            $"Username={username};" +
            $"Password={password};" +
            "SSL Mode=Require;" +
            "Trust Server Certificate=true";
    }
}
