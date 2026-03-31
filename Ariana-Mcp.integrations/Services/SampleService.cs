using Ariana_Mcp.Integrations.AraianLab;

namespace Ariana_Mcp.integrations.Services;

public sealed class SampleService(IHttpClientFactory httpClientFactory)
{
    public async Task<string> GetSampleByIdAsync(
        string sampleId,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(sampleId))
            return "sampleId must not be empty.";

        var client = CreateClient();
        var response = await GetAsStringAsync(
            client,
            $"Rest/Opd/Proben/{Uri.EscapeDataString(sampleId)}/",
            cancellationToken);
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

    private sealed record HttpResult(bool IsSuccess, string Body);
}

