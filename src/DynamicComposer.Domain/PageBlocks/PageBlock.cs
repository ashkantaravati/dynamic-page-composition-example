namespace DynamicComposer.Domain.PageBlocks;

public abstract class PageBlock
{
    public Guid Id { get; init; }
    public int SortOrder { get; init; }
}
