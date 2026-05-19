using Ecommerce.Modules.Catalog.Contracts;
using Ecommerce.Modules.Catalog.Queries;

namespace Ecommerce.Modules.Catalog.Queries.GetCatalogCategories;

public sealed class GetCatalogCategoriesQueryHandler
{
    private readonly CatalogProductProjector _projector;

    public GetCatalogCategoriesQueryHandler(CatalogProductProjector projector)
    {
        _projector = projector;
    }

    public Task<CatalogCategoriesResponse> HandleAsync(
        GetCatalogCategoriesQuery query,
        CancellationToken cancellationToken = default)
    {
        var categories = _projector.BrowseableCategories();
        return Task.FromResult(new CatalogCategoriesResponse(categories));
    }
}
