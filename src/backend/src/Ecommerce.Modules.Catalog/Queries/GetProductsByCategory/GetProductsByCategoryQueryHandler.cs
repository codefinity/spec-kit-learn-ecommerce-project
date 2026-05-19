using Ecommerce.Modules.Catalog.Contracts;
using Ecommerce.Modules.Catalog.Queries;
using Ecommerce.Modules.Catalog.Validation;

namespace Ecommerce.Modules.Catalog.Queries.GetProductsByCategory;

public sealed class GetProductsByCategoryQueryHandler
{
    private readonly CatalogProductProjector _projector;

    public GetProductsByCategoryQueryHandler(CatalogProductProjector projector)
    {
        _projector = projector;
    }

    public Task<CatalogResultSetResponse> HandleAsync(
        GetProductsByCategoryQuery query,
        CancellationToken cancellationToken = default)
    {
        var validated = CatalogRequestValidation.ValidateListQuery(
            query.Search,
            query.CategorySlug,
            query.Cursor,
            query.Limit);

        if (!validated.IsValid || validated.Value is null)
        {
            throw new ArgumentException("Category products query is invalid.");
        }

        return _projector.ListAsync(validated.Value, cancellationToken);
    }
}
