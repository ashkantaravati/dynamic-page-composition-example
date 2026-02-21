namespace DynamicComposer.Domain;

public abstract class PageBlock
{
    public Guid Id { get; init; }
    public int SortOrder { get; init; }
}

public sealed class ContentBlock : PageBlock
{
    public string Heading { get; init; } = string.Empty;
    public string Body { get; init; } = string.Empty;
}

public sealed class HeroBlock : PageBlock
{
    public string Headline { get; init; } = string.Empty;
    public string SubHeadline { get; init; } = string.Empty;
    public string? CtaText { get; init; }
    public string? CtaUrl { get; init; }
}

public sealed class FaqBlock : PageBlock
{
    public string Heading { get; init; } = string.Empty;
    public IReadOnlyCollection<FaqItem> Items { get; init; } = [];
}

public sealed record FaqItem(string Question, string Answer);

public sealed class RelatedPagesBlock : PageBlock
{
    public string Heading { get; init; } = string.Empty;
    public IReadOnlyCollection<Guid> RelatedPageIds { get; init; } = [];
}

public sealed class FeaturedRelatedPageBlock : PageBlock
{
    public string Heading { get; init; } = string.Empty;
    public Guid FeaturedPageId { get; init; }
}

public sealed class TwoColumnBlock : PageBlock
{
    public string Heading { get; init; } = string.Empty;
    public IReadOnlyCollection<PageBlock> MainBlocks { get; init; } = [];
    public IReadOnlyCollection<PageBlock> AsideBlocks { get; init; } = [];
}
