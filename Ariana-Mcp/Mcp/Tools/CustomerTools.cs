using System.ComponentModel;
using Ariana_Mcp.Integrations.AraianLab;
using Ariana_Mcp.integrations.Services;
using ModelContextProtocol.Server;

namespace Ariana_Mcp.Mcp.Tools;

[McpServerToolType]
public sealed class CustomerTools(CustomerService customerService)
{
    [McpServerTool(Name = "customer_by_name", ReadOnly = true, Idempotent = true, Destructive = false)]
    [Description("Looks up a customer by name and returns the customer JSON from ArianaLab.")]
    public async Task<string> CustomerByName(string name, CancellationToken cancellationToken = default)
        => await customerService.GetCustomerByNameAsync(name, cancellationToken).ConfigureAwait(false);

    [McpServerTool(Name = "customer_info_by_id", ReadOnly = true, Idempotent = true, Destructive = false)]
    [Description("Returns KundenInformationen JSON for a customer id (e.g. /Rest/Mad/Kunden/14197/KundenInformationen).")]
    public async Task<string> CustomerInfoById(string customerId, CancellationToken cancellationToken = default)
        => await customerService.GetCustomerInfoAsync(customerId, cancellationToken).ConfigureAwait(false);

    [McpServerTool(Name = "all_customers", ReadOnly = true, Idempotent = true, Destructive = false)]
    [Description("Returns all customers as JSON from ArianaLab. Warning: this call can take longer due to payload size.")]
    public async Task<string> GetAllCustomers(CancellationToken cancellationToken = default)
        => await customerService.GetAllCustomersAsync(cancellationToken).ConfigureAwait(false);
}

