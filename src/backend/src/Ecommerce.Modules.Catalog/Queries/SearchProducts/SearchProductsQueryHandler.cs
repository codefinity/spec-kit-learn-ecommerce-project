using Ecommerce.Modules.Catalog.Contracts;
using Ecommerce.Modules.Catalog.Queries;
using Ecommerce.Modules.Catalog.Validation;

namespace Ecommerce.Modules.Catalog.Queries.SearchProducts;

public sealed class SearchProductsQueryHandler
{
    private readonly CatalogProductProjector _projector;

    public SearchProductsQueryHandler(CatalogProductProjector projector)
    {
        _projector = projector;
    }

    public Task<CatalogResultSetResponse> HandleAsync(
        SearchProductsQuery query,
        CancellationToken cancellationToken = default)
    {
        var validated = CatalogRequestValidation.ValidateListQuery(query.Search, null, query.Cursor, query.Limit);
        if (!validated.IsValid || validated.Value is null)
        {
            throw new ArgumentException("Search products query is invalid.");
        }

        return _projector.ListAsync(validated.Value, cancellationToken);
    }
}
