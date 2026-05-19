namespace Ecommerce.Modules.Catalog.Data.Entities;

public sealed class Product
{
    public Guid Id { get; init; }
    public string Slug { get; init; } = "";
    public string Name { get; init; } = "";
    public string Description { get; init; } = "";
    public decimal PriceAmount { get; init; }
    public string Currency { get; init; } = "USD";
    public string? PrimaryImageUrl { get; init; }
    public ProductPublishStatus PublishStatus { get; init; }
    public bool IsFeatured { get; init; }
    public Guid PrimaryCategoryId { get; init; }
    public DateTimeOffset CreatedAt { get; init; }
    public DateTimeOffset? PublishedAt { get; init; }
}
