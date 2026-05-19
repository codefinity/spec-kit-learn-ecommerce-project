using Ecommerce.Modules.Catalog.Queries.GetCatalogCategories;
using Xunit;

namespace Ecommerce.Modules.Catalog.Tests.Queries;

public sealed class GetCatalogCategoriesQueryHandlerTests
{
    [Fact]
    public async Task HandleAsync_returns_only_browseable_categories_alphabetically()
    {
        var handler = CatalogTestFactory.CategoriesHandler();

        var result = await handler.HandleAsync(new GetCatalogCategoriesQuery());

        Assert.DoesNotContain(result.Items, category => category.Slug == "clearance");
        Assert.Equal(result.Items.Select(category => category.Name).OrderBy(name => name), result.Items.Select(category => category.Name));
    }
}
