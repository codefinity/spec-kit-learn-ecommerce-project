using System.Text.RegularExpressions;
using Ecommerce.Modules.Catalog.Queries;

namespace Ecommerce.Modules.Catalog.Validation;

public sealed record CatalogValidationResult<T>(
    bool IsValid,
    T? Value,
    IReadOnlyDictionary<string, string[]> Errors);

public static partial class CatalogRequestValidation
{
    private const int SearchMaxLength = 120;
    private const int SlugMaxLength = 100;

    public static CatalogValidationResult<ValidatedCatalogListQuery> ValidateListQuery(
        string? search,
        string? category,
        string? cursor,
        int? limit)
    {
        var errors = new Dictionary<string, string[]>(StringComparer.OrdinalIgnoreCase);
        var normalizedSearch = NormalizeOptional(search);
        var normalizedCategory = NormalizeOptional(category)?.ToLowerInvariant();

        if (normalizedSearch?.Length > SearchMaxLength)
        {
            errors["search"] = [$"Search text cannot exceed {SearchMaxLength} characters."];
        }

        if (normalizedCategory is not null && !IsValidSlug(normalizedCategory))
        {
            errors["category"] = ["Category must be a URL-safe slug."];
        }

        if (!CatalogCursor.TryDecode(cursor, out var decodedCursor))
        {
            errors["cursor"] = ["Cursor is invalid."];
        }

        var normalizedLimit = limit ?? CatalogQueryDefaults.DefaultLimit;
        if (normalizedLimit < 1 || normalizedLimit > CatalogQueryDefaults.MaxLimit)
        {
            errors["limit"] = [$"Limit must be between 1 and {CatalogQueryDefaults.MaxLimit}."];
        }

        if (errors.Count > 0)
        {
            return new CatalogValidationResult<ValidatedCatalogListQuery>(false, null, errors);
        }

        return new CatalogValidationResult<ValidatedCatalogListQuery>(
            true,
            new ValidatedCatalogListQuery(normalizedSearch, normalizedCategory, decodedCursor.Offset, normalizedLimit),
            errors);
    }

    public static CatalogValidationResult<string> ValidateProductSlug(string? slug)
    {
        var normalized = NormalizeOptional(slug)?.ToLowerInvariant();
        if (normalized is null || !IsValidSlug(normalized))
        {
            return new CatalogValidationResult<string>(
                false,
                null,
                new Dictionary<string, string[]> { ["productSlug"] = ["Product slug must be a URL-safe slug."] });
        }

        return new CatalogValidationResult<string>(
            true,
            normalized,
            new Dictionary<string, string[]>());
    }

    private static string? NormalizeOptional(string? value)
    {
        var normalized = value?.Trim();
        return string.IsNullOrWhiteSpace(normalized) ? null : normalized;
    }

    private static bool IsValidSlug(string slug) =>
        slug.Length <= SlugMaxLength && SlugRegex().IsMatch(slug);

    [GeneratedRegex("^[a-z0-9]+(?:-[a-z0-9]+)*$", RegexOptions.CultureInvariant)]
    private static partial Regex SlugRegex();
}
