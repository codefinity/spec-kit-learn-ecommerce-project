namespace Ecommerce.Modules.Catalog.Contracts;

public sealed record ProductDetailResponse(
    Guid Id,
    string Slug,
    string Name,
    string Description,
    MoneyResponse Price,
    CategorySummaryResponse PrimaryCategory,
    IReadOnlyList<CategorySummaryResponse> SecondaryCategories,
    IReadOnlyList<ProductImageResponse> Images,
    bool IsFeatured,
    AvailabilitySummaryResponse Availability);
