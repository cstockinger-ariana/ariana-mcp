using System.ComponentModel;
using ariana_mcp.Integrations.AraianLab;
using ModelContextProtocol.Server;

namespace ariana_mcp.Mcp.Resources;

[McpServerResourceType]
public sealed class CustomerResources(IHttpClientFactory httpClientFactory)
{
    [McpServerResource(
        UriTemplate = "customers://all",
        Name = "all_customers",
        MimeType = "application/json")]
    [Description("Returns all customers as JSON from ArianaLab. Warning: this resource can take longer due to payload size.")]
    public async Task<string> GetAllCustomers(CancellationToken cancellationToken = default)
    {
        var client = httpClientFactory.CreateClient(ArianaLabHttp.ClientName);

        using var response = await client.GetAsync("Rest/Mad/Kunden", cancellationToken).ConfigureAwait(false);
        var body = await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);

        if (!response.IsSuccessStatusCode)
        {
            return $"HTTP {(int)response.StatusCode} {response.ReasonPhrase}: {body}";
        }

        return body;
    }
}
