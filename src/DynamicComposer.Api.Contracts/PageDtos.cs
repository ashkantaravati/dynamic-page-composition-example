namespace DynamicComposer.Api.Contracts;

public sealed record PageSummaryDto(Guid Id, string Title, string Slug, string? Description, IReadOnlyCollection<string> BlockTypes);

public sealed record PageDetailsDto(
    Guid Id,
    string Title,
    string Slug,
    MetaDataDto MetaData,
    IReadOnlyCollection<PageBlockDto> Blocks);

public sealed record MetaDataDto(string? Title, string? Description, string? Keywords);

public sealed record PageListFilterDto(string? Search = null, string? Slug = null, int? Skip = null, int? Take = null);

public sealed record UpsertPageDto(string Title, string Slug, MetaDataDto MetaData, IReadOnlyCollection<PageBlockDto> Blocks);
