using System.Text.Json.Serialization;

namespace DynamicComposer.Api.Contracts;

[JsonPolymorphic(TypeDiscriminatorPropertyName = "type")]
[JsonDerivedType(typeof(ContentBlockDto), typeDiscriminator: "content")]
[JsonDerivedType(typeof(HeroBlockDto), typeDiscriminator: "hero")]
[JsonDerivedType(typeof(FaqBlockDto), typeDiscriminator: "faq")]
[JsonDerivedType(typeof(RelatedPagesBlockDto), typeDiscriminator: "related-pages")]
[JsonDerivedType(typeof(FeaturedRelatedPageBlockDto), typeDiscriminator: "featured-related-page")]
[JsonDerivedType(typeof(TwoColumnBlockDto), typeDiscriminator: "two-column")]
public abstract record PageBlockDto(Guid Id, int SortOrder);

public sealed record ContentBlockDto(Guid Id, int SortOrder, string Heading, string Body) : PageBlockDto(Id, SortOrder);
public sealed record HeroBlockDto(Guid Id, int SortOrder, string Headline, string SubHeadline, string? CtaText, string? CtaUrl) : PageBlockDto(Id, SortOrder);
public sealed record FaqItemDto(string Question, string Answer);
public sealed record FaqBlockDto(Guid Id, int SortOrder, string Heading, IReadOnlyCollection<FaqItemDto> Items) : PageBlockDto(Id, SortOrder);
public sealed record RelatedPagesBlockDto(Guid Id, int SortOrder, string Heading, IReadOnlyCollection<Guid> RelatedPageIds) : PageBlockDto(Id, SortOrder);
public sealed record FeaturedRelatedPageBlockDto(Guid Id, int SortOrder, string Heading, Guid FeaturedPageId) : PageBlockDto(Id, SortOrder);
public sealed record TwoColumnBlockDto(Guid Id, int SortOrder, string Heading, IReadOnlyCollection<PageBlockDto> MainBlocks, IReadOnlyCollection<PageBlockDto> AsideBlocks) : PageBlockDto(Id, SortOrder);
