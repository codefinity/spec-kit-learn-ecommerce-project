using Ecommerce.Modules.Inventory.Contracts;
using Microsoft.Extensions.DependencyInjection;

namespace Ecommerce.Modules.Inventory;

public static class InventoryModule
{
    public static IServiceCollection AddInventoryModule(this IServiceCollection services)
    {
        services.AddSingleton<IProductAvailabilityReader, StaticProductAvailabilityReader>();
        return services;
    }
}
