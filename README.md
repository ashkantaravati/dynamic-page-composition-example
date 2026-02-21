# DynamicComposer

A multi-project .NET solution that demonstrates dynamic page composition using predefined blocks and rendering in a separate front-office host.

## Projects

- `DynamicComposer.Api.Contracts`: DTOs and API contracts shared between hosts.
- `DynamicComposer.Domain`: domain model (`Page`, `MetaData`, and block types).
- `DynamicComposer.Persistence.EF`: EF Core DbContext and SQLite mapping.
- `DynamicComposer.BackOffice.Host`: ASP.NET Core Web API with Angular SPA for page management.
- `DynamicComposer.FrontOffice.Host`: Razor Pages app that consumes the API and renders blocks via ViewComponents.

## API endpoints

- `GET /api/pages` with query params from `PageListFilterDto`.
- `GET /api/pages/{id}` for full details.
- Additional CRUD endpoints to support back-office management.

## Running both apps

```bash
dotnet watch --project src/DynamicComposer.BackOffice.Host
dotnet watch --project src/DynamicComposer.FrontOffice.Host
```

Back-office API defaults to `https://localhost:7070`, front-office defaults to `https://localhost:7060`.
