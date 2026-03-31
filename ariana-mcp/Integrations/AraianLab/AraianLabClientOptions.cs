namespace ariana_mcp.Integrations.AraianLab;

public sealed class AraianLabClientOptions
{
    public const string SectionName = "AraianLab";

    public string User { get; set; } = "";

    public string Password { get; set; } = "";

    /// <summary>Optional base URL for requests (e.g. https://api.example.com).</summary>
    public string? BaseUrl { get; set; }
}

