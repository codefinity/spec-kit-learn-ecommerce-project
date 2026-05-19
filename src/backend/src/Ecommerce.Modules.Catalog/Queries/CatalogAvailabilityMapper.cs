using Ecommerce.Modules.Catalog.Contracts;
using Ecommerce.Modules.Inventory.Contracts;

namespace Ecommerce.Modules.Catalog.Queries;

public static class CatalogAvailabilityMapper
{
    public static async Task<AvailabilitySummaryResponse> MapAsync(
        IProductAvailabilityReader reader,
        Guid productId,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var summary = await reader.GetAvailabilityAsync(productId, cancellationToken);
            if (summary is null || string.IsNullOrWhiteSpace(summary.DisplayText))
            {
                return Unavailable();
            }

            return new AvailabilitySummaryResponse(summary.State.ToString(), summary.DisplayText);
        }
        catch
        {
            return Unavailable();
        }
    }

    public static AvailabilitySummaryResponse Unavailable() =>
        new("Unavailable", "availability unavailable");
}
