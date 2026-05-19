using Ecommerce.Modules.Catalog.Data.Entities;

namespace Ecommerce.Modules.Catalog.Data;

public sealed class CatalogDbContext
{
    public CatalogDbContext(
        IEnumerable<Product> products,
        IEnumerable<Category> categories,
        IEnumerable<ProductImage> productImages,
        IEnumerable<ProductSecondaryCategory> secondaryCategories)
    {
        Products = products.ToList();
        Categories = categories.ToList();
        ProductImages = productImages.ToList();
        ProductSecondaryCategories = secondaryCategories.ToList();
    }

    public List<Product> Products { get; }
    public List<Category> Categories { get; }
    public List<ProductImage> ProductImages { get; }
    public List<ProductSecondaryCategory> ProductSecondaryCategories { get; }

    public static CatalogDbContext Seeded() => CatalogSeedData.CreateContext();
}
