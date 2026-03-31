using System.Reflection;
using ariana_mcp.Configuration;
using ariana_mcp.Integrations.AraianLab;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

var argv = new List<string>(args);

var builder = WebApplication.CreateBuilder(argv.ToArray());

builder.ConfigureAppSettings(argv.ToArray());
builder.ConfigureLoggingSettings();

builder.Services.AddAraianLabHttpClient(builder.Configuration);

builder.Services
    .AddMcpServer()
    .WithHttpTransport(o => o.Stateless = true)
    .WithPromptsFromAssembly()
    .WithToolsFromAssembly();

var app = builder.Build();

app.MapGet("/", () =>
{
    var asm = Assembly.GetExecutingAssembly();
    var name = asm.GetName().Name ?? "ariana-mcp";
    var version =
        asm.GetCustomAttribute<AssemblyInformationalVersionAttribute>()?.InformationalVersion
        ?? asm.GetName().Version?.ToString()
        ?? "unknown";

    return Results.Ok(new { name, version });
});

app.MapGet("/health", () => Results.Ok(new { status = "ok" }));
app.MapMcp("/mcp");

await app.RunAsync();

