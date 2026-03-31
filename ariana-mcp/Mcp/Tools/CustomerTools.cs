using System.ComponentModel;
using System.Text.Json;
using ariana_mcp.Integrations.AraianLab;
using ModelContextProtocol.Server;

namespace ariana_mcp.Mcp.Tools;

[McpServerToolType]
public sealed class CustomerTools(IHttpClientFactory httpClientFactory)
{
    [McpServerTool(Name = "customer_by_name", ReadOnly = true, Idempotent = true, Destructive = false)]
    [Description("Looks up a customer by name and returns the customer JSON from ArianaLab.")]
    public async Task<string> CustomerByName(string name, CancellationToken cancellationToken = default)
    {
        var client = httpClientFactory.CreateClient(ArianaLabHttp.ClientName);

        using var responseCustomerInfo = await client
            .GetAsync(
                $"Rest/Mad/Kunden/KundenInformationen/ByName?name={Uri.EscapeDataString(name)}",
                cancellationToken)
            .ConfigureAwait(false);

        var body = await responseCustomerInfo.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);

        if (!responseCustomerInfo.IsSuccessStatusCode)
        {
            return $"HTTP {(int)responseCustomerInfo.StatusCode} {responseCustomerInfo.ReasonPhrase}: {body}";
        }

        var kundeId = TryGetKundeId(body);

        using var responseCustomer = await client.GetAsync($"Rest/Mad/Kunden/{kundeId}", cancellationToken)
            .ConfigureAwait(false);

        body = await responseCustomer.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);
        return body;
    }

    private static string TryGetKundeId(string json)
    {
        if (string.IsNullOrWhiteSpace(json))
            return "(empty response)";

        try
        {
            using var doc = JsonDocument.Parse(json);
            if (!doc.RootElement.TryGetProperty("KundeId", out var kundeId))
                return $"KundeId not found in response: {json}";

            if (kundeId.ValueKind == JsonValueKind.Null)
                return "(KundeId is null)";

            return kundeId.GetString() ?? kundeId.ToString();
        }
        catch (JsonException ex)
        {
            return $"Invalid JSON ({ex.Message}): {json}";
        }
    }
}

