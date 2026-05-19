namespace Ecommerce.Modules.Catalog.Contracts;

public sealed record CatalogResultSetResponse(
    IReadOnlyList<ProductCardResponse> Items,
    int TotalCount,
    string? NextCursor,
    bool HasMore,
    string? AppliedSearch,
    string? AppliedCategorySlug);
