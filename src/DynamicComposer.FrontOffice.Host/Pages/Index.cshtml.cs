using DynamicComposer.Api.Contracts;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DynamicComposer.FrontOffice.Host.Pages;

public sealed class IndexModel(IHttpClientFactory httpClientFactory) : PageModel
{
    public IReadOnlyCollection<PageSummaryDto> Pages { get; private set; } = [];

    public async Task OnGetAsync(CancellationToken cancellationToken)
    {
        var client = httpClientFactory.CreateClient("BackOfficeApi");
        Pages = await client.GetFromJsonAsync<IReadOnlyCollection<PageSummaryDto>>("/api/pages", cancellationToken) ?? [];
    }
}
