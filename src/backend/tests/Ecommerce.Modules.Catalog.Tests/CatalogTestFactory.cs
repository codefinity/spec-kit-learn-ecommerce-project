using Ecommerce.Modules.Catalog.Data;
using Ecommerce.Modules.Catalog.Queries;
using Ecommerce.Modules.Catalog.Queries.BrowseProducts;
using Ecommerce.Modules.Catalog.Queries.GetCatalogCategories;
using Ecommerce.Modules.Catalog.Queries.GetProductDetail;
using Ecommerce.Modules.Catalog.Queries.GetProductsByCategory;
using Ecommerce.Modules.Catalog.Queries.SearchProducts;
using Ecommerce.Modules.Inventory.Contracts;

namespace Ecommerce.Modules.Catalog.Tests;

internal static class CatalogTestFactory
{
    public static CatalogDbContext CreateDb() => CatalogDbContext.Seeded();

    public static CatalogProductProjector CreateProjector(IProductAvailabilityReader? reader = null) =>
        new(CreateDb(), reader ?? new StaticProductAvailabilityReader());

    public static BrowseProductsQueryHandler BrowseHandler(IProductAvailabilityReader? reader = null) =>
        new(CreateProjector(reader));

    public static SearchProductsQueryHandler SearchHandler(IProductAvailabilityReader? reader = null) =>
        new(CreateProjector(reader));

    public static GetProductsByCategoryQueryHandler CategoryHandler(IProductAvailabilityReader? reader = null) =>
        new(CreateProjector(reader));

    public static GetProductDetailQueryHandler DetailHandler(IProductAvailabilityReader? reader = null) =>
        new(CreateProjector(reader));

    public static GetCatalogCategoriesQueryHandler CategoriesHandler() =>
        new(CreateProjector());
}
