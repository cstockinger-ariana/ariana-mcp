namespace Ariana_Mcp.integrations.Services;

public sealed class SampleService(IHttpClientFactory httpClientFactory)
    : ArianaLabServiceBase(httpClientFactory)
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
}

