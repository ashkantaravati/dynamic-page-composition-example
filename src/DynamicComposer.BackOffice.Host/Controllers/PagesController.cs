using DynamicComposer.Api.Contracts;
using DynamicComposer.Domain;
using DynamicComposer.Persistence.EF;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DynamicComposer.BackOffice.Host.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class PagesController(DynamicComposerDbContext dbContext) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyCollection<PageSummaryDto>>> ListAsync([FromQuery] PageListFilterDto filter, CancellationToken cancellationToken)
    {
        var query = dbContext.Pages.AsNoTracking().AsQueryable();

        if (!string.IsNullOrWhiteSpace(filter.Search))
        {
            query = query.Where(x => x.Title.Contains(filter.Search));
        }

        if (!string.IsNullOrWhiteSpace(filter.Slug))
        {
            query = query.Where(x => x.Slug == filter.Slug);
        }

        if (filter.Skip is > 0)
        {
            query = query.Skip(filter.Skip.Value);
        }

        if (filter.Take is > 0)
        {
            query = query.Take(filter.Take.Value);
        }

        var pages = await query
            .Select(x => new PageSummaryDto(x.Id, x.Title, x.Slug, x.MetaData.Description, x.Blocks.Select(b => b.GetType().Name).ToArray()))
            .ToListAsync(cancellationToken);

        return Ok(pages);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<PageDetailsDto>> GetAsync(Guid id, CancellationToken cancellationToken)
    {
        var page = await dbContext.Pages.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        if (page is null)
        {
            return NotFound();
        }

        return Ok(ToDetails(page));
    }

    [HttpPost]
    public async Task<ActionResult<PageDetailsDto>> CreateAsync([FromBody] UpsertPageDto request, CancellationToken cancellationToken)
    {
        var page = new Page(Guid.NewGuid(), request.Title, request.Slug, ToDomain(request.MetaData), request.Blocks.Select(ToDomain));
        dbContext.Pages.Add(page);
        await dbContext.SaveChangesAsync(cancellationToken);
        return CreatedAtAction(nameof(GetAsync), new { id = page.Id }, ToDetails(page));
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult<PageDetailsDto>> UpdateAsync(Guid id, [FromBody] UpsertPageDto request, CancellationToken cancellationToken)
    {
        var page = await dbContext.Pages.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        if (page is null)
        {
            return NotFound();
        }

        page.ReplaceContent(request.Title, request.Slug, ToDomain(request.MetaData), request.Blocks.Select(ToDomain));
        await dbContext.SaveChangesAsync(cancellationToken);

        return Ok(ToDetails(page));
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        var page = await dbContext.Pages.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        if (page is null)
        {
            return NotFound();
        }

        dbContext.Pages.Remove(page);
        await dbContext.SaveChangesAsync(cancellationToken);
        return NoContent();
    }

    private static PageDetailsDto ToDetails(Page page)
        => new(page.Id, page.Title, page.Slug, new MetaDataDto(page.MetaData.Title, page.MetaData.Description, page.MetaData.Keywords), page.Blocks.Select(ToDto).ToArray());

    private static MetaData ToDomain(MetaDataDto dto) => new() { Title = dto.Title, Description = dto.Description, Keywords = dto.Keywords };

    private static PageBlock ToDomain(PageBlockDto dto)
        => dto switch
        {
            ContentBlockDto x => new ContentBlock { Id = x.Id, SortOrder = x.SortOrder, Heading = x.Heading, Body = x.Body },
            HeroBlockDto x => new HeroBlock { Id = x.Id, SortOrder = x.SortOrder, Headline = x.Headline, SubHeadline = x.SubHeadline, CtaText = x.CtaText, CtaUrl = x.CtaUrl },
            FaqBlockDto x => new FaqBlock { Id = x.Id, SortOrder = x.SortOrder, Heading = x.Heading, Items = x.Items.Select(i => new FaqItem(i.Question, i.Answer)).ToArray() },
            RelatedPagesBlockDto x => new RelatedPagesBlock { Id = x.Id, SortOrder = x.SortOrder, Heading = x.Heading, RelatedPageIds = x.RelatedPageIds },
            FeaturedRelatedPageBlockDto x => new FeaturedRelatedPageBlock { Id = x.Id, SortOrder = x.SortOrder, Heading = x.Heading, FeaturedPageId = x.FeaturedPageId },
            TwoColumnBlockDto x => new TwoColumnBlock { Id = x.Id, SortOrder = x.SortOrder, Heading = x.Heading, MainBlocks = x.MainBlocks.Select(ToDomain).ToArray(), AsideBlocks = x.AsideBlocks.Select(ToDomain).ToArray() },
            _ => throw new InvalidOperationException($"Unsupported block DTO type: {dto.GetType().Name}")
        };

    private static PageBlockDto ToDto(PageBlock block)
        => block switch
        {
            ContentBlock x => new ContentBlockDto(x.Id, x.SortOrder, x.Heading, x.Body),
            HeroBlock x => new HeroBlockDto(x.Id, x.SortOrder, x.Headline, x.SubHeadline, x.CtaText, x.CtaUrl),
            FaqBlock x => new FaqBlockDto(x.Id, x.SortOrder, x.Heading, x.Items.Select(i => new FaqItemDto(i.Question, i.Answer)).ToArray()),
            RelatedPagesBlock x => new RelatedPagesBlockDto(x.Id, x.SortOrder, x.Heading, x.RelatedPageIds),
            FeaturedRelatedPageBlock x => new FeaturedRelatedPageBlockDto(x.Id, x.SortOrder, x.Heading, x.FeaturedPageId),
            TwoColumnBlock x => new TwoColumnBlockDto(x.Id, x.SortOrder, x.Heading, x.MainBlocks.Select(ToDto).ToArray(), x.AsideBlocks.Select(ToDto).ToArray()),
            _ => throw new InvalidOperationException($"Unsupported block type: {block.GetType().Name}")
        };
}
