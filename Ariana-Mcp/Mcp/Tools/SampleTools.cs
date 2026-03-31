using System.ComponentModel;
using Ariana_Mcp.integrations.Services;
using ModelContextProtocol.Server;

namespace Ariana_Mcp.Mcp.Tools;

[McpServerToolType]
public sealed class SampleTools(SampleService sampleService)
{
    [McpServerTool(Name = "sample_by_id", ReadOnly = true, Idempotent = true, Destructive = false)]
    [Description("Looks up a sample (Probe) by id (e.g. 26-0318054) and returns the JSON from ArianaLab.")]
    public async Task<string> SampleById(string sampleId, CancellationToken cancellationToken = default)
        => await sampleService.GetSampleByIdAsync(sampleId, cancellationToken).ConfigureAwait(false);
}

