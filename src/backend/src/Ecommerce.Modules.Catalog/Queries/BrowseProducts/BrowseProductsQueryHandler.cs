using Ecommerce.Modules.Catalog.Contracts;
using Ecommerce.Modules.Catalog.Queries;
using Ecommerce.Modules.Catalog.Validation;

namespace Ecommerce.Modules.Catalog.Queries.BrowseProducts;

public sealed class BrowseProductsQueryHandler
{
    private readonly CatalogProductProjector _projector;

    public BrowseProductsQueryHandler(CatalogProductProjector projector)
    {
        _projector = projector;
    }

    public Task<CatalogResultSetResponse> HandleAsync(
        BrowseProductsQuery query,
        CancellationToken cancellationToken = default)
    {
        var validated = CatalogRequestValidation.ValidateListQuery(null, null, query.Cursor, query.Limit);
        if (!validated.IsValid || validated.Value is null)
        {
            throw new ArgumentException("Browse products query is invalid.");
        }

        return _projector.ListAsync(validated.Value, cancellationToken);
    }
}
