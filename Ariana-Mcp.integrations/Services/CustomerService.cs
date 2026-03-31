using System.Text.Json;
using Ariana_Mcp.Integrations.AraianLab;

namespace Ariana_Mcp.integrations.Services;

public sealed class CustomerService(IHttpClientFactory httpClientFactory)
{
    public async Task<string> GetCustomerByNameAsync(string name, CancellationToken cancellationToken = default)
    {
        var client = CreateClient();
        var customerInfo = await GetAsStringAsync(
            client,
            $"Rest/Mad/Kunden/KundenInformationen/ByName?name={Uri.EscapeDataString(name)}",
            cancellationToken);

        if (!customerInfo.IsSuccess)
            return customerInfo.Body;

        var kundeId = TryGetKundeId(customerInfo.Body);
        var customer = await GetAsStringAsync(client, $"Rest/Mad/Kunden/{kundeId}", cancellationToken);
        return customer.Body;
    }

    public async Task<string> GetAllCustomersAsync(CancellationToken cancellationToken = default)
    {
        var client = CreateClient();
        var response = await GetAsStringAsync(client, "Rest/Mad/Kunden", cancellationToken);
        return response.Body;
    }

    private HttpClient CreateClient() => httpClientFactory.CreateClient(ArianaLabHttp.ClientName);

    private static async Task<HttpResult> GetAsStringAsync(
        HttpClient client,
        string requestUri,
        CancellationToken cancellationToken)
    {
        using var response = await client.GetAsync(requestUri, cancellationToken).ConfigureAwait(false);
        var body = await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);
        return response.IsSuccessStatusCode
            ? new HttpResult(true, body)
            : new HttpResult(
                false,
                $"HTTP {(int)response.StatusCode} {response.ReasonPhrase}: {body}");
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

    private sealed record HttpResult(bool IsSuccess, string Body);
}
