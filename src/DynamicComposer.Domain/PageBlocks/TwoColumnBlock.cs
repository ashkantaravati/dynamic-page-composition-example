namespace DynamicComposer.Domain.PageBlocks;

public class TwoColumnBlock : PageBlock
{
    public string Heading { get; init; } = string.Empty;
    public IReadOnlyCollection<PageBlock> MainBlocks { get; init; } = [];
    public IReadOnlyCollection<PageBlock> AsideBlocks { get; init; } = [];
}
