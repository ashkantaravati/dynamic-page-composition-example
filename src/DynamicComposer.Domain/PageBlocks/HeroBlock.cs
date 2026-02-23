namespace DynamicComposer.Domain.PageBlocks;

public class HeroBlock : PageBlock
{
    public string Headline { get; init; } = string.Empty;
    public string SubHeadline { get; init; } = string.Empty;
    public string? CtaText { get; init; }
    public string? CtaUrl { get; init; }
}
