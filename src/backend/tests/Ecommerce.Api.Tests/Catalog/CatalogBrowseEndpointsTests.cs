using System.Net;
using System.Net.Http.Json;
using Ecommerce.Modules.Catalog.Contracts;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace Ecommerce.Api.Tests.Catalog;

public sealed class CatalogBrowseEndpointsTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public CatalogBrowseEndpointsTests(WebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task Get_products_returns_published_ordered_result_set()
    {
        var response = await _client.GetFromJsonAsync<CatalogResultSetResponse>("/api/catalog/products?limit=5");

        Assert.NotNull(response);
        Assert.Equal("trail-runner-shoe", response.Items[0].Slug);
        Assert.True(response.HasMore);
        Assert.NotNull(response.NextCursor);
    }

    [Fact]
    public async Task Get_product_detail_returns_published_product()
    {
        var response = await _client.GetFromJsonAsync<ProductDetailResponse>("/api/catalog/products/trail-runner-shoe");

        Assert.NotNull(response);
        Assert.Equal("Trail Runner Shoe", response.Name);
        Assert.Contains(response.SecondaryCategories, category => category.Slug == "accessories");
    }

    [Fact]
    public async Task Get_product_detail_returns_not_found_for_unknown_or_unpublished_product()
    {
        var missing = await _client.GetAsync("/api/catalog/products/missing-product");
        var unpublished = await _client.GetAsync("/api/catalog/products/hidden-sample-product");

        Assert.Equal(HttpStatusCode.NotFound, missing.StatusCode);
        Assert.Equal(HttpStatusCode.NotFound, unpublished.StatusCode);
    }
}
