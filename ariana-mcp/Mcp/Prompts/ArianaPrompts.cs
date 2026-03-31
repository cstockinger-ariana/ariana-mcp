using System.ComponentModel;
using ModelContextProtocol.Server;

namespace ariana_mcp.Mcp.Prompts;

[McpServerPromptType]
public static class ArianaPrompts
{
    [McpServerPrompt(Name = "about_ariana_mcp", Title = "About Ariana MCP")]
    [Description("Briefly describes what this MCP server can do.")]
    public static string About()
        => "This server provides ArianaLab customer lookups as MCP tools/resources. Try the tool `customer_by_name` with a customer name.";
}

