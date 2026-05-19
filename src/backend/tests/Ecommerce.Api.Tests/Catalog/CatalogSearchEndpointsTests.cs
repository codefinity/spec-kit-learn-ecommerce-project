using System.Net;
using System.Net.Http.Json;
using Ecommerce.Modules.Catalog.Contracts;
using Xunit;

namespace Ecommerce.Api.Tests.Catalog;

public sealed class CatalogSearchEndpointsTests : IClassFixture<Microsoft.AspNetCore.Mvc.Testing.WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public CatalogSearchEndpointsTests(Microsoft.AspNetCore.Mvc.Testing.WebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task Get_products_search_returns_case_insensitive_matches()
    {
        var response = await _client.GetFromJsonAsync<CatalogResultSetResponse>("/api/catalog/products?search=RUN");

        Assert.NotNull(response);
        Assert.Contains(response.Items, item => item.Slug == "trail-runner-shoe");
        Assert.Equal("RUN", response.AppliedSearch);
    }

    [Fact]
    public async Task Get_products_search_rejects_overlong_query()
    {
        var search = new string('x', 121);
        var response = await _client.GetAsync($"/api/catalog/products?search={search}");

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }
}
