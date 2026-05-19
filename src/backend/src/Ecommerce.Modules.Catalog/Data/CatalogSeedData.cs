using Ecommerce.Modules.Catalog.Data.Entities;

namespace Ecommerce.Modules.Catalog.Data;

public static class CatalogSeedData
{
    public static readonly Guid FootwearId = Guid.Parse("10000000-0000-0000-0000-000000000001");
    public static readonly Guid ApparelId = Guid.Parse("10000000-0000-0000-0000-000000000002");
    public static readonly Guid AccessoriesId = Guid.Parse("10000000-0000-0000-0000-000000000003");
    public static readonly Guid ClearanceId = Guid.Parse("10000000-0000-0000-0000-000000000004");

    public static readonly Guid TrailShoeId = Guid.Parse("20000000-0000-0000-0000-000000000001");
    public static readonly Guid CityJacketId = Guid.Parse("20000000-0000-0000-0000-000000000002");
    public static readonly Guid TravelMugId = Guid.Parse("20000000-0000-0000-0000-000000000003");
    public static readonly Guid HiddenSampleId = Guid.Parse("20000000-0000-0000-0000-000000000004");

    public static CatalogDbContext CreateContext()
    {
        var now = new DateTimeOffset(2026, 5, 19, 0, 0, 0, TimeSpan.Zero);

        var categories = new[]
        {
            new Category { Id = FootwearId, Slug = "footwear", Name = "Footwear", IsBrowseable = true },
            new Category { Id = ApparelId, Slug = "apparel", Name = "Apparel", IsBrowseable = true },
            new Category { Id = AccessoriesId, Slug = "accessories", Name = "Accessories", IsBrowseable = true },
            new Category { Id = ClearanceId, Slug = "clearance", Name = "Clearance", IsBrowseable = false }
        };

        var products = new List<Product>
        {
            new Product
            {
                Id = TrailShoeId,
                Slug = "trail-runner-shoe",
                Name = "Trail Runner Shoe",
                Description = "Lightweight shoe for long trail walks and weekend runs.",
                PriceAmount = 89.99m,
                Currency = "USD",
                PrimaryImageUrl = "/images/trail-runner-shoe.svg",
                PublishStatus = ProductPublishStatus.Published,
                IsFeatured = true,
                PrimaryCategoryId = FootwearId,
                CreatedAt = now.AddDays(-10),
                PublishedAt = now.AddDays(-9)
            },
            new Product
            {
                Id = CityJacketId,
                Slug = "city-rain-jacket",
                Name = "City Rain Jacket",
                Description = "Water-resistant jacket with a quiet everyday fit.",
                PriceAmount = 129.00m,
                Currency = "USD",
                PrimaryImageUrl = "/images/city-rain-jacket.svg",
                PublishStatus = ProductPublishStatus.Published,
                IsFeatured = false,
                PrimaryCategoryId = ApparelId,
                CreatedAt = now.AddDays(-8),
                PublishedAt = now.AddDays(-7)
            },
            new Product
            {
                Id = TravelMugId,
                Slug = "insulated-travel-mug",
                Name = "Insulated Travel Mug",
                Description = "Keeps coffee hot through commutes, errands, and desk mornings.",
                PriceAmount = 24.50m,
                Currency = "USD",
                PrimaryImageUrl = null,
                PublishStatus = ProductPublishStatus.Published,
                IsFeatured = false,
                PrimaryCategoryId = AccessoriesId,
                CreatedAt = now.AddDays(-6),
                PublishedAt = now.AddDays(-5)
            },
            new Product
            {
                Id = HiddenSampleId,
                Slug = "hidden-sample-product",
                Name = "Hidden Sample Product",
                Description = "Unpublished product used to test shopper visibility.",
                PriceAmount = 10.00m,
                Currency = "USD",
                PrimaryImageUrl = "/images/hidden.svg",
                PublishStatus = ProductPublishStatus.Unpublished,
                IsFeatured = true,
                PrimaryCategoryId = ClearanceId,
                CreatedAt = now.AddDays(-4),
                PublishedAt = null
            }
        };

        products.AddRange(Enumerable.Range(1, 30).Select(index =>
        {
            var categoryId = (index % 3) switch
            {
                0 => FootwearId,
                1 => ApparelId,
                _ => AccessoriesId
            };

            return new Product
            {
                Id = Guid.Parse($"20000000-0000-0000-0000-{index + 10:000000000000}"),
                Slug = $"everyday-catalog-item-{index:00}",
                Name = $"Everyday Catalog Item {index:00}",
                Description = $"Seeded tutorial product {index:00} for browsing, search, and scrolling.",
                PriceAmount = 18.00m + index,
                Currency = "USD",
                PrimaryImageUrl = null,
                PublishStatus = ProductPublishStatus.Published,
                IsFeatured = false,
                PrimaryCategoryId = categoryId,
                CreatedAt = now.AddDays(-20 - index),
                PublishedAt = now.AddDays(-19 - index)
            };
        }));

        var images = products
            .Where(product => !string.IsNullOrWhiteSpace(product.PrimaryImageUrl))
            .Select(product => new ProductImage
            {
                Id = Guid.NewGuid(),
                ProductId = product.Id,
                Url = product.PrimaryImageUrl!,
                AltText = product.Name,
                SortOrder = 0,
                IsPrimary = true
            })
            .ToArray();

        var secondaryCategories = new[]
        {
            new ProductSecondaryCategory { ProductId = TrailShoeId, CategoryId = AccessoriesId },
            new ProductSecondaryCategory { ProductId = CityJacketId, CategoryId = AccessoriesId },
            new ProductSecondaryCategory { ProductId = Guid.Parse("20000000-0000-0000-0000-000000000012"), CategoryId = FootwearId }
        };

        return new CatalogDbContext(products, categories, images, secondaryCategories);
    }
}
