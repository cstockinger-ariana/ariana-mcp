using System.Net.Http.Headers;
using System.Text;
using Microsoft.Extensions.Options;

namespace ariana_mcp.Integrations.AraianLab;

public sealed class AraianLabAuthHandler(IOptions<AraianLabClientOptions> options) : DelegatingHandler
{
    private readonly AraianLabClientOptions _options = options.Value;

    protected override Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        if (!string.IsNullOrEmpty(_options.User))
        {
            var raw = $"{_options.User}:{_options.Password}";
            var token = Convert.ToBase64String(Encoding.UTF8.GetBytes(raw));
            request.Headers.Authorization = new AuthenticationHeaderValue("Basic", token);
        }

        return base.SendAsync(request, cancellationToken);
    }
}

