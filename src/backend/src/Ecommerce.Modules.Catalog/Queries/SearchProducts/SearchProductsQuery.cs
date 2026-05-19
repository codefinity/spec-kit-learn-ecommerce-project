namespace Ecommerce.Modules.Catalog.Queries.SearchProducts;

public sealed record SearchProductsQuery(string Search, string? Cursor = null, int? Limit = null);
