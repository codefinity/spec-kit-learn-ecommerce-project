using Ecommerce.Modules.Catalog.Queries.GetProductDetail;
using Xunit;

namespace Ecommerce.Modules.Catalog.Tests.Queries;

public sealed class GetProductDetailQueryHandlerTests
{
    [Fact]
    public async Task HandleAsync_returns_published_product_detail_with_secondary_categories()
    {
        var handler = CatalogTestFactory.DetailHandler();

        var result = await handler.HandleAsync(new GetProductDetailQuery("trail-runner-shoe"));

        Assert.NotNull(result);
        Assert.Equal("Trail Runner Shoe", result.Name);
        Assert.Equal("Footwear", result.PrimaryCategory.Name);
        Assert.Contains(result.SecondaryCategories, category => category.Slug == "accessories");
        Assert.NotEmpty(result.Images);
    }

    [Fact]
    public async Task HandleAsync_returns_null_for_unknown_slug()
    {
        var handler = CatalogTestFactory.DetailHandler();

        var result = await handler.HandleAsync(new GetProductDetailQuery("missing-product"));

        Assert.Null(result);
    }

    [Fact]
    public async Task HandleAsync_returns_null_for_unpublished_slug()
    {
        var handler = CatalogTestFactory.DetailHandler();

        var result = await handler.HandleAsync(new GetProductDetailQuery("hidden-sample-product"));

        Assert.Null(result);
    }

    [Fact]
    public async Task HandleAsync_returns_empty_images_when_product_has_no_image()
    {
        var handler = CatalogTestFactory.DetailHandler();

        var result = await handler.HandleAsync(new GetProductDetailQuery("insulated-travel-mug"));

        Assert.NotNull(result);
        Assert.Empty(result.Images);
    }
}
