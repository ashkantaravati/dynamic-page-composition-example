namespace DynamicComposer.Domain.PageBlocks;

public class RelatedPagesBlock : PageBlock
{
    public string Heading { get; init; } = string.Empty;
    public IReadOnlyCollection<Guid> RelatedPageIds { get; init; } = [];
}
