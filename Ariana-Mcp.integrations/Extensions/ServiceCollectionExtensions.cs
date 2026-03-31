using Ariana_Mcp.integrations.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Ariana_Mcp.Integrations.AraianLab;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddAraianLabHttpClient(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.Configure<AraianLabClientOptions>(
            configuration.GetSection(AraianLabClientOptions.SectionName));
        services.AddTransient<AraianLabAuthHandler>();
        services.AddTransient<CustomerService>();
        services.AddTransient<SampleService>();
        services.AddHttpClient(ArianaLabHttp.ClientName, (sp, client) =>
            {
                var options = sp.GetRequiredService<IOptions<AraianLabClientOptions>>().Value;
                if (!string.IsNullOrWhiteSpace(options.BaseUrl))
                    client.BaseAddress = new Uri(options.BaseUrl);
            })
            .AddHttpMessageHandler<AraianLabAuthHandler>();

        return services;
    }
}
