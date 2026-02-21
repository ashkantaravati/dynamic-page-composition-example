using DynamicComposer.Api.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace DynamicComposer.FrontOffice.Host.ViewComponents;

public sealed class FeaturedRelatedPageBlockViewComponent(IHttpClientFactory httpClientFactory) : ViewComponent
{
    public async Task<IViewComponentResult> InvokeAsync(FeaturedRelatedPageBlockDto block)
    {
        var client = httpClientFactory.CreateClient("BackOfficeApi");
        var page = await client.GetFromJsonAsync<PageDetailsDto>($"/api/pages/{block.FeaturedPageId}", HttpContext.RequestAborted);
        return View((block, page));
    }
}
