using Ecommerce.Modules.Catalog.Queries.GetProductsByCategory;
using Xunit;

namespace Ecommerce.Modules.Catalog.Tests.Queries;

public sealed class GetProductsByCategoryQueryHandlerTests
{
    [Fact]
    public async Task HandleAsync_matches_primary_category()
    {
        var handler = CatalogTestFactory.CategoryHandler();

        var result = await handler.HandleAsync(new GetProductsByCategoryQuery("footwear", Limit: 60));

        Assert.Contains(result.Items, item => item.Slug == "trail-runner-shoe");
        Assert.Equal("footwear", result.AppliedCategorySlug);
    }

    [Fact]
    public async Task HandleAsync_matches_secondary_category()
    {
        var handler = CatalogTestFactory.CategoryHandler();

        var result = await handler.HandleAsync(new GetProductsByCategoryQuery("accessories", Limit: 60));

        Assert.Contains(result.Items, item => item.Slug == "trail-runner-shoe");
        Assert.Contains(result.Items, item => item.Slug == "city-rain-jacket");
    }

    [Fact]
    public async Task HandleAsync_returns_empty_for_non_browseable_or_missing_category()
    {
        var handler = CatalogTestFactory.CategoryHandler();

        var clearance = await handler.HandleAsync(new GetProductsByCategoryQuery("clearance"));
        var missing = await handler.HandleAsync(new GetProductsByCategoryQuery("missing-category"));

        Assert.Empty(clearance.Items);
        Assert.Empty(missing.Items);
    }

    [Fact]
    public async Task HandleAsync_combines_search_and_category_filters()
    {
        var handler = CatalogTestFactory.CategoryHandler();

        var result = await handler.HandleAsync(new GetProductsByCategoryQuery("footwear", "trail"));

        var item = Assert.Single(result.Items);
        Assert.Equal("trail-runner-shoe", item.Slug);
        Assert.Equal("trail", result.AppliedSearch);
    }
}
