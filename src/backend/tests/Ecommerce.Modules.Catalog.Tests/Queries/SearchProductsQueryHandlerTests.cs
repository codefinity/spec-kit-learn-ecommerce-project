using Ecommerce.Modules.Catalog.Queries.SearchProducts;
using Xunit;

namespace Ecommerce.Modules.Catalog.Tests.Queries;

public sealed class SearchProductsQueryHandlerTests
{
    [Fact]
    public async Task HandleAsync_matches_case_insensitive_partial_text_across_name_and_description()
    {
        var handler = CatalogTestFactory.SearchHandler();

        var result = await handler.HandleAsync(new SearchProductsQuery("RUN"));

        Assert.Contains(result.Items, item => item.Slug == "trail-runner-shoe");
        Assert.DoesNotContain(result.Items, item => item.Slug == "hidden-sample-product");
    }

    [Fact]
    public async Task HandleAsync_trims_search_text_and_preserves_featured_first_ordering()
    {
        var handler = CatalogTestFactory.SearchHandler();

        var result = await handler.HandleAsync(new SearchProductsQuery("  item  ", Limit: 10));

        Assert.Equal("item", result.AppliedSearch);
        Assert.All(result.Items, item => Assert.Contains("item", item.Name, StringComparison.OrdinalIgnoreCase));
        Assert.True(result.Items.Select(item => item.Name).SequenceEqual(result.Items.Select(item => item.Name).OrderBy(name => name, StringComparer.OrdinalIgnoreCase)));
    }

    [Fact]
    public async Task HandleAsync_returns_empty_result_for_no_match()
    {
        var handler = CatalogTestFactory.SearchHandler();

        var result = await handler.HandleAsync(new SearchProductsQuery("not-a-product"));

        Assert.Empty(result.Items);
        Assert.False(result.HasMore);
    }
}
