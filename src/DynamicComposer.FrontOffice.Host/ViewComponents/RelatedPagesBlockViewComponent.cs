using DynamicComposer.Api.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace DynamicComposer.FrontOffice.Host.ViewComponents;

public sealed class RelatedPagesBlockViewComponent(IHttpClientFactory httpClientFactory) : ViewComponent
{
    public async Task<IViewComponentResult> InvokeAsync(RelatedPagesBlockDto block)
    {
        var client = httpClientFactory.CreateClient("BackOfficeApi");
        var pages = await client.GetFromJsonAsync<IReadOnlyCollection<PageSummaryDto>>("/api/pages", HttpContext.RequestAborted) ?? [];
        var related = pages.Where(x => block.RelatedPageIds.Contains(x.Id)).ToArray();
        return View((block, related));
    }
}
