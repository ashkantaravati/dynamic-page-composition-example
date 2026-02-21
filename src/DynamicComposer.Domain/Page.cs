namespace DynamicComposer.Domain;

public sealed class Page
{
    private readonly List<PageBlock> _blocks = [];

    public Guid Id { get; private set; }
    public string Title { get; private set; }
    public string Slug { get; private set; }
    public MetaData MetaData { get; private set; }
    public IReadOnlyCollection<PageBlock> Blocks => _blocks.OrderBy(x => x.SortOrder).ToArray();

    private Page()
    {
        Title = string.Empty;
        Slug = string.Empty;
        MetaData = new MetaData();
    }

    public Page(Guid id, string title, string slug, MetaData metaData, IEnumerable<PageBlock> blocks)
    {
        Id = id;
        Title = title;
        Slug = slug;
        MetaData = metaData;
        _blocks = blocks.OrderBy(x => x.SortOrder).ToList();
    }

    public void ReplaceContent(string title, string slug, MetaData metaData, IEnumerable<PageBlock> blocks)
    {
        Title = title;
        Slug = slug;
        MetaData = metaData;

        _blocks.Clear();
        _blocks.AddRange(blocks.OrderBy(x => x.SortOrder));
    }
}
