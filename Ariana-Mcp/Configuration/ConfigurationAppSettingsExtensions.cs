using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Steeltoe.Extensions.Configuration.Placeholder;

namespace Ariana_Mcp.Configuration;

internal static class ConfigurationAppSettingsExtensions
{
    internal static WebApplicationBuilder ConfigureAppSettings(this WebApplicationBuilder builder, string[] args)
    {
        var contentRootPath = builder.Environment.ContentRootPath;
        var environmentName = builder.Environment.EnvironmentName;

        builder.Configuration.RemoveDefaultConfigSources();

        builder.Configuration
            .SetBasePath(contentRootPath)
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddJsonFile($"appsettings.{environmentName}.json", optional: true, reloadOnChange: true)
            .AddJsonFile("appsettings.override.json", optional: true, reloadOnChange: true)
            .AddEnvironmentVariables();

        if (args.Length > 0)
        {
            builder.Configuration.AddCommandLine(args);
        }

        // Wrap all existing sources so ${placeholders} get resolved.
        builder.AddPlaceholderResolver();

        builder.Services.AddHttpClient();

        return builder;
    }

    private static void RemoveDefaultConfigSources(this IConfigurationBuilder configurationBuilder)
    {
        // WebApplication.CreateBuilder pre-populates sources (appsettings*, env vars, command line, etc).
        // We clear them so we can re-add in a deterministic order before enabling placeholder resolution.
        configurationBuilder.Sources.Clear();
    }
}

