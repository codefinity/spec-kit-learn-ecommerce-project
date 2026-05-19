using Ecommerce.Modules.Catalog.Queries.BrowseProducts;
using Xunit;

namespace Ecommerce.Modules.Catalog.Tests.Queries;

public sealed class BrowseProductsQueryHandlerTests
{
    [Fact]
    public async Task HandleAsync_returns_only_published_products_with_featured_first_ordering()
    {
        var handler = CatalogTestFactory.BrowseHandler();

        var result = await handler.HandleAsync(new BrowseProductsQuery(Limit: 60));

        Assert.DoesNotContain(result.Items, product => product.Slug == "hidden-sample-product");
        Assert.Equal("trail-runner-shoe", result.Items[0].Slug);
        Assert.True(result.Items[0].IsFeatured);
        Assert.Equal(result.Items.Count, result.Items.Select(product => product.Id).Distinct().Count());
    }

    [Fact]
    public async Task HandleAsync_returns_cursor_batches_for_infinite_scroll()
    {
        var handler = CatalogTestFactory.BrowseHandler();

        var firstPage = await handler.HandleAsync(new BrowseProductsQuery(Limit: 5));
        var secondPage = await handler.HandleAsync(new BrowseProductsQuery(firstPage.NextCursor, 5));

        Assert.True(firstPage.HasMore);
        Assert.NotNull(firstPage.NextCursor);
        Assert.Equal(5, firstPage.Items.Count);
        Assert.Equal(5, secondPage.Items.Count);
        Assert.Empty(firstPage.Items.Select(item => item.Id).Intersect(secondPage.Items.Select(item => item.Id)));
    }

    [Fact]
    public async Task HandleAsync_leaves_primary_image_null_when_product_has_no_image()
    {
        var handler = CatalogTestFactory.SearchHandler();

        var result = await handler.HandleAsync(new("travel mug"));

        var mug = Assert.Single(result.Items);
        Assert.Equal("insulated-travel-mug", mug.Slug);
        Assert.Null(mug.PrimaryImage);
    }
}
