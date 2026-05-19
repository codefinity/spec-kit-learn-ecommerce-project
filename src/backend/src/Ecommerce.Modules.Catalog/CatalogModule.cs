using Ecommerce.Modules.Catalog.Data;
using Ecommerce.Modules.Catalog.Queries;
using Ecommerce.Modules.Catalog.Queries.BrowseProducts;
using Ecommerce.Modules.Catalog.Queries.GetCatalogCategories;
using Ecommerce.Modules.Catalog.Queries.GetProductDetail;
using Ecommerce.Modules.Catalog.Queries.GetProductsByCategory;
using Ecommerce.Modules.Catalog.Queries.SearchProducts;
using Microsoft.Extensions.DependencyInjection;

namespace Ecommerce.Modules.Catalog;

public static class CatalogModule
{
    public static IServiceCollection AddCatalogModule(this IServiceCollection services)
    {
        services.AddSingleton(CatalogDbContext.Seeded());
        services.AddScoped<CatalogProductProjector>();
        services.AddScoped<BrowseProductsQueryHandler>();
        services.AddScoped<SearchProductsQueryHandler>();
        services.AddScoped<GetProductsByCategoryQueryHandler>();
        services.AddScoped<GetProductDetailQueryHandler>();
        services.AddScoped<GetCatalogCategoriesQueryHandler>();
        return services;
    }
}
