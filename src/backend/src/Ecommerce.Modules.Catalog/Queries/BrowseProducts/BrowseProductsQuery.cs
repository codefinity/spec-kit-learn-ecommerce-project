namespace Ecommerce.Modules.Catalog.Queries.BrowseProducts;

public sealed record BrowseProductsQuery(string? Cursor = null, int? Limit = null);
