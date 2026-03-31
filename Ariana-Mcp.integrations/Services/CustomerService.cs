using System.Text.Json;

namespace Ariana_Mcp.integrations.Services;

public sealed class CustomerService(IHttpClientFactory httpClientFactory)
    : ArianaLabServiceBase(httpClientFactory)
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

    public async Task<string> GetCustomerInfoAsync(
        string customerId,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(customerId))
            return "customerId must not be empty.";

        var client = CreateClient();
        var response = await GetAsStringAsync(
            client,
            $"Rest/Mad/Kunden/{Uri.EscapeDataString(customerId)}/KundenInformationen",
            cancellationToken);
        return response.Body;
    }

    public async Task<string> GetAllCustomersAsync(CancellationToken cancellationToken = default)
    {
        var client = CreateClient();
        var response = await GetAsStringAsync(client, "Rest/Mad/Kunden", cancellationToken);
        return response.Body;
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
