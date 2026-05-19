using Ecommerce.Modules.Catalog.Contracts;
using Ecommerce.Modules.Catalog.Data;
using Ecommerce.Modules.Catalog.Data.Entities;
using Ecommerce.Modules.Inventory.Contracts;

namespace Ecommerce.Modules.Catalog.Queries;

public sealed class CatalogProductProjector
{
    private readonly CatalogDbContext _db;
    private readonly IProductAvailabilityReader _availabilityReader;

    public CatalogProductProjector(CatalogDbContext db, IProductAvailabilityReader availabilityReader)
    {
        _db = db;
        _availabilityReader = availabilityReader;
    }

    public async Task<CatalogResultSetResponse> ListAsync(
        ValidatedCatalogListQuery query,
        CancellationToken cancellationToken = default)
    {
        var products = _db.Products
            .Where(product => product.PublishStatus == ProductPublishStatus.Published);

        if (!string.IsNullOrWhiteSpace(query.Search))
        {
            products = products.Where(product =>
                product.Name.Contains(query.Search, StringComparison.OrdinalIgnoreCase) ||
                product.Description.Contains(query.Search, StringComparison.OrdinalIgnoreCase));
        }

        if (!string.IsNullOrWhiteSpace(query.CategorySlug))
        {
            var category = _db.Categories.SingleOrDefault(candidate =>
                candidate.IsBrowseable &&
                string.Equals(candidate.Slug, query.CategorySlug, StringComparison.OrdinalIgnoreCase));

            products = category is null
                ? []
                : products.Where(product => IsInCategory(product, category.Id));
        }

        var ordered = products
            .OrderByDescending(product => product.IsFeatured)
            .ThenBy(product => product.Name, StringComparer.OrdinalIgnoreCase)
            .ThenBy(product => product.Id)
            .ToList();

        var page = ordered
            .Skip(query.Offset)
            .Take(query.Limit)
            .ToList();

        var cards = new List<ProductCardResponse>(page.Count);
        foreach (var product in page)
        {
            cards.Add(await ToCardAsync(product, cancellationToken));
        }

        var nextOffset = query.Offset + page.Count;
        var hasMore = nextOffset < ordered.Count;

        return new CatalogResultSetResponse(
            cards,
            ordered.Count,
            hasMore ? CatalogCursor.Encode(nextOffset) : null,
            hasMore,
            query.Search,
            query.CategorySlug);
    }

    public async Task<ProductDetailResponse?> DetailAsync(
        string slug,
        CancellationToken cancellationToken = default)
    {
        var product = _db.Products.SingleOrDefault(candidate =>
            candidate.PublishStatus == ProductPublishStatus.Published &&
            string.Equals(candidate.Slug, slug, StringComparison.OrdinalIgnoreCase));

        if (product is null)
        {
            return null;
        }

        var primaryCategory = GetCategory(product.PrimaryCategoryId);
        var secondaryCategories = _db.ProductSecondaryCategories
            .Where(link => link.ProductId == product.Id)
            .Select(link => GetCategory(link.CategoryId))
            .Where(category => category is not null)
            .Select(category => ToCategorySummary(category!))
            .OrderBy(category => category.Name, StringComparer.OrdinalIgnoreCase)
            .ToList();

        var images = _db.ProductImages
            .Where(image => image.ProductId == product.Id)
            .OrderByDescending(image => image.IsPrimary)
            .ThenBy(image => image.SortOrder)
            .Select(image => new ProductImageResponse(image.Url, image.AltText))
            .ToList();

        return new ProductDetailResponse(
            product.Id,
            product.Slug,
            product.Name,
            product.Description,
            new MoneyResponse(product.PriceAmount, product.Currency),
            ToCategorySummary(primaryCategory),
            secondaryCategories,
            images,
            product.IsFeatured,
            await CatalogAvailabilityMapper.MapAsync(_availabilityReader, product.Id, cancellationToken));
    }

    public IReadOnlyList<CategorySummaryResponse> BrowseableCategories() =>
        _db.Categories
            .Where(category => category.IsBrowseable)
            .OrderBy(category => category.Name, StringComparer.OrdinalIgnoreCase)
            .Select(ToCategorySummary)
            .ToList();

    private async Task<ProductCardResponse> ToCardAsync(
        Product product,
        CancellationToken cancellationToken)
    {
        var primaryCategory = GetCategory(product.PrimaryCategoryId);
        var primaryImage = _db.ProductImages
            .Where(image => image.ProductId == product.Id)
            .OrderByDescending(image => image.IsPrimary)
            .ThenBy(image => image.SortOrder)
            .Select(image => new ProductImageResponse(image.Url, image.AltText))
            .FirstOrDefault();

        return new ProductCardResponse(
            product.Id,
            product.Slug,
            product.Name,
            new MoneyResponse(product.PriceAmount, product.Currency),
            primaryImage,
            ToCategorySummary(primaryCategory),
            product.IsFeatured,
            await CatalogAvailabilityMapper.MapAsync(_availabilityReader, product.Id, cancellationToken));
    }

    private bool IsInCategory(Product product, Guid categoryId) =>
        product.PrimaryCategoryId == categoryId ||
        _db.ProductSecondaryCategories.Any(link => link.ProductId == product.Id && link.CategoryId == categoryId);

    private Category GetCategory(Guid categoryId) =>
        _db.Categories.Single(category => category.Id == categoryId);

    private static CategorySummaryResponse ToCategorySummary(Category category) =>
        new(category.Id, category.Slug, category.Name);
}
