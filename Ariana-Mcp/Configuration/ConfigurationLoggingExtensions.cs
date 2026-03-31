using Microsoft.AspNetCore.Builder;
using Serilog;

namespace Ariana_Mcp.Configuration;

public static class ConfigurationLoggingExtensions
{
    internal static WebApplicationBuilder ConfigureLoggingSettings(this WebApplicationBuilder builder)
    {
        builder.Host.UseSerilog((ctx, services, loggerConfiguration) =>
        {
            loggerConfiguration
                .ReadFrom.Configuration(ctx.Configuration)
                .ReadFrom.Services(services)
                .Enrich.FromLogContext();
        });
        
        return builder;
    }
}