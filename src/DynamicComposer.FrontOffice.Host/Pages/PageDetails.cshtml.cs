using DynamicComposer.Api.Contracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DynamicComposer.FrontOffice.Host.Pages;

public sealed class PageDetailsModel(IHttpClientFactory httpClientFactory) : PageModel
{
    public PageDetailsDto? PageDetails { get; private set; }

    public async Task<IActionResult> OnGetAsync(Guid id, string slug, CancellationToken cancellationToken)
    {
        var client = httpClientFactory.CreateClient("BackOfficeApi");
        PageDetails = await client.GetFromJsonAsync<PageDetailsDto>($"/api/pages/{id}", cancellationToken);
        if (PageDetails is null)
        {
            return NotFound();
        }

        if (!string.Equals(PageDetails.Slug, slug, StringComparison.OrdinalIgnoreCase))
        {
            return RedirectToPage("/PageDetails", new { id, slug = PageDetails.Slug });
        }

        return Page();
    }
}
