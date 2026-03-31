using Ariana_Mcp.Integrations.AraianLab;

namespace Ariana_Mcp.integrations.Services;

public abstract class ArianaLabServiceBase(IHttpClientFactory httpClientFactory)
{
    protected HttpClient CreateClient() => httpClientFactory.CreateClient(ArianaLabHttp.ClientName);

    protected static async Task<HttpResult> GetAsStringAsync(
        HttpClient client,
        string requestUri,
        CancellationToken cancellationToken)
    {
        using var response = await client.GetAsync(requestUri, cancellationToken).ConfigureAwait(false);
        var body = await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);
        return response.IsSuccessStatusCode
            ? new HttpResult(true, body)
            : new HttpResult(false, $"HTTP {(int)response.StatusCode} {response.ReasonPhrase}: {body}");
    }

    protected sealed record HttpResult(bool IsSuccess, string Body);
}

