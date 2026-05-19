namespace Ecommerce.Modules.Catalog.Queries.GetProductsByCategory;

public sealed record GetProductsByCategoryQuery(
    string CategorySlug,
    string? Search = null,
    string? Cursor = null,
    int? Limit = null);
