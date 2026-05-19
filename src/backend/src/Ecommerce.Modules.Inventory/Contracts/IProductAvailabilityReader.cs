namespace Ecommerce.Modules.Inventory.Contracts;

public enum AvailabilityState
{
    InStock,
    LowStock,
    OutOfStock,
    Unavailable
}

public sealed record ProductAvailabilitySummary(
    Guid ProductId,
    AvailabilityState State,
    string DisplayText);

public interface IProductAvailabilityReader
{
    Task<ProductAvailabilitySummary?> GetAvailabilityAsync(
        Guid productId,
        CancellationToken cancellationToken = default);
}
