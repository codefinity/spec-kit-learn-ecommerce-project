namespace Ecommerce.Modules.Catalog.Queries;

public static class CatalogQueryDefaults
{
    public const int DefaultLimit = 24;
    public const int MaxLimit = 60;
}

public sealed record ValidatedCatalogListQuery(
    string? Search,
    string? CategorySlug,
    int Offset,
    int Limit);

public sealed record CatalogCursor(int Offset)
{
    public static string Encode(int offset) => offset.ToString();

    public static bool TryDecode(string? cursor, out CatalogCursor decoded)
    {
        if (string.IsNullOrWhiteSpace(cursor))
        {
            decoded = new CatalogCursor(0);
            return true;
        }

        if (int.TryParse(cursor, out var offset) && offset >= 0)
        {
            decoded = new CatalogCursor(offset);
            return true;
        }

        decoded = new CatalogCursor(0);
        return false;
    }
}
