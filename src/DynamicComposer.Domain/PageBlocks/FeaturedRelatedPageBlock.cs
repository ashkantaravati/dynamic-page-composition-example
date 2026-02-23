namespace DynamicComposer.Domain.PageBlocks;

public class FeaturedRelatedPageBlock : PageBlock
{
    public string Heading { get; init; } = string.Empty;
    public Guid FeaturedPageId { get; init; }
}
