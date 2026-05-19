namespace Ecommerce.Modules.Catalog.Contracts;

public sealed record ProductCardResponse(
    Guid Id,
    string Slug,
    string Name,
    MoneyResponse Price,
    ProductImageResponse? PrimaryImage,
    CategorySummaryResponse PrimaryCategory,
    bool IsFeatured,
    AvailabilitySummaryResponse Availability);
