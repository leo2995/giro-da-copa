namespace GiroDaCopa.Api.Configuration;

public sealed class FrontendAccessSettings
{
    public const string SectionName = "Frontend";
    public const string ApiKeyHeaderName = "X-Frontend-Key";
    public const string ApiKeyEnvName = "FRONTEND_API_KEY";

    public string? ApiKey { get; set; }
}
