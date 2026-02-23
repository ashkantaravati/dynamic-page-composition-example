namespace DynamicComposer.Domain.PageBlocks;

public class FaqBlock : PageBlock
{
    public string Heading { get; init; } = string.Empty;
    public IReadOnlyCollection<FaqItem> Items { get; init; } = [];
}
