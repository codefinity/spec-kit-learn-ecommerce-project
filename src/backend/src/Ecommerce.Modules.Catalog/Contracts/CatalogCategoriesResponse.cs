namespace Ecommerce.Modules.Catalog.Contracts;

public sealed record CatalogCategoriesResponse(IReadOnlyList<CategorySummaryResponse> Items);
