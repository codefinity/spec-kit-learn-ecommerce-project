using Ecommerce.Modules.Catalog.Contracts;
using Ecommerce.Modules.Catalog.Queries;
using Ecommerce.Modules.Catalog.Validation;

namespace Ecommerce.Modules.Catalog.Queries.GetProductDetail;

public sealed class GetProductDetailQueryHandler
{
    private readonly CatalogProductProjector _projector;

    public GetProductDetailQueryHandler(CatalogProductProjector projector)
    {
        _projector = projector;
    }

    public Task<ProductDetailResponse?> HandleAsync(
        GetProductDetailQuery query,
        CancellationToken cancellationToken = default)
    {
        var validated = CatalogRequestValidation.ValidateProductSlug(query.ProductSlug);
        if (!validated.IsValid || validated.Value is null)
        {
            throw new ArgumentException("Product slug is invalid.");
        }

        return _projector.DetailAsync(validated.Value, cancellationToken);
    }
}
