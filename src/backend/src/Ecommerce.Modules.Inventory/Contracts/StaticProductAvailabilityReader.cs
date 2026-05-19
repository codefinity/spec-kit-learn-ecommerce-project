namespace Ecommerce.Modules.Inventory.Contracts;

public sealed class StaticProductAvailabilityReader : IProductAvailabilityReader
{
    private readonly IReadOnlyDictionary<Guid, ProductAvailabilitySummary> _summaries;

    public StaticProductAvailabilityReader(IReadOnlyDictionary<Guid, ProductAvailabilitySummary>? summaries = null)
    {
        _summaries = summaries ?? new Dictionary<Guid, ProductAvailabilitySummary>();
    }

    public Task<ProductAvailabilitySummary?> GetAvailabilityAsync(
        Guid productId,
        CancellationToken cancellationToken = default)
    {
        if (_summaries.TryGetValue(productId, out var summary))
        {
            return Task.FromResult<ProductAvailabilitySummary?>(summary);
        }

        return Task.FromResult<ProductAvailabilitySummary?>(
            new ProductAvailabilitySummary(productId, AvailabilityState.InStock, "In stock"));
    }
}
