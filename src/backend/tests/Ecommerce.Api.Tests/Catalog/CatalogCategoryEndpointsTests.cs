using System.Net.Http.Json;
using Ecommerce.Modules.Catalog.Contracts;
using Xunit;

namespace Ecommerce.Api.Tests.Catalog;

public sealed class CatalogCategoryEndpointsTests : IClassFixture<Microsoft.AspNetCore.Mvc.Testing.WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public CatalogCategoryEndpointsTests(Microsoft.AspNetCore.Mvc.Testing.WebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task Get_categories_returns_browseable_options()
    {
        var response = await _client.GetFromJsonAsync<CatalogCategoriesResponse>("/api/catalog/categories");

        Assert.NotNull(response);
        Assert.Contains(response.Items, category => category.Slug == "footwear");
        Assert.DoesNotContain(response.Items, category => category.Slug == "clearance");
    }

    [Fact]
    public async Task Get_products_category_combines_category_and_search()
    {
        var response = await _client.GetFromJsonAsync<CatalogResultSetResponse>(
            "/api/catalog/products?category=footwear&search=trail");

        Assert.NotNull(response);
        var item = Assert.Single(response.Items);
        Assert.Equal("trail-runner-shoe", item.Slug);
    }
}
