using Ecommerce.Modules.Catalog.Queries.BrowseProducts;
using Ecommerce.Modules.Catalog.Queries.GetProductDetail;
using Ecommerce.Modules.Inventory.Contracts;
using Xunit;

namespace Ecommerce.Modules.Catalog.Tests.Integration;

public sealed class InventoryAvailabilityFallbackTests
{
    [Fact]
    public async Task BrowseProducts_uses_unavailable_fallback_when_inventory_returns_null()
    {
        var handler = CatalogTestFactory.BrowseHandler(new NullAvailabilityReader());

        var result = await handler.HandleAsync(new BrowseProductsQuery(Limit: 1));

        Assert.Equal("Unavailable", result.Items[0].Availability.State);
        Assert.Equal("availability unavailable", result.Items[0].Availability.DisplayText);
    }

    [Fact]
    public async Task ProductDetail_uses_unavailable_fallback_when_inventory_fails()
    {
        var handler = CatalogTestFactory.DetailHandler(new FailingAvailabilityReader());

        var result = await handler.HandleAsync(new GetProductDetailQuery("trail-runner-shoe"));

        Assert.NotNull(result);
        Assert.Equal("Unavailable", result.Availability.State);
        Assert.Equal("availability unavailable", result.Availability.DisplayText);
    }

    private sealed class NullAvailabilityReader : IProductAvailabilityReader
    {
        public Task<ProductAvailabilitySummary?> GetAvailabilityAsync(Guid productId, CancellationToken cancellationToken = default) =>
            Task.FromResult<ProductAvailabilitySummary?>(null);
    }

    private sealed class FailingAvailabilityReader : IProductAvailabilityReader
    {
        public Task<ProductAvailabilitySummary?> GetAvailabilityAsync(Guid productId, CancellationToken cancellationToken = default) =>
            throw new InvalidOperationException("Inventory unavailable");
    }
}
