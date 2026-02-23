namespace DynamicComposer.Domain.PageBlocks;

public class ContentBlock : PageBlock
{
    public string Heading { get; init; } = string.Empty;
    public string Body { get; init; } = string.Empty;
}
