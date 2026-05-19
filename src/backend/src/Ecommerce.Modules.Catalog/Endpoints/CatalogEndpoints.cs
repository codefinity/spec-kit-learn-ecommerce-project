using Ecommerce.Modules.Catalog.Queries.BrowseProducts;
using Ecommerce.Modules.Catalog.Queries.GetCatalogCategories;
using Ecommerce.Modules.Catalog.Queries.GetProductDetail;
using Ecommerce.Modules.Catalog.Queries.GetProductsByCategory;
using Ecommerce.Modules.Catalog.Queries.SearchProducts;
using Ecommerce.Modules.Catalog.Validation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace Ecommerce.Modules.Catalog.Endpoints;

public static class CatalogEndpoints
{
    public static RouteGroupBuilder MapCatalogEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/catalog");

        group.MapGet("/products", BrowseProductsAsync)
            .WithName("BrowseCatalogProducts");

        group.MapGet("/products/{productSlug}", GetProductDetailAsync)
            .WithName("GetCatalogProductDetail");

        group.MapGet("/categories", GetCategoriesAsync)
            .WithName("GetCatalogCategories");

        return group;
    }

    private static async Task<Results<Ok<object>, ValidationProblem>> BrowseProductsAsync(
        [FromQuery] string? search,
        [FromQuery] string? category,
        [FromQuery] string? cursor,
        [FromQuery] int? limit,
        BrowseProductsQueryHandler browseHandler,
        SearchProductsQueryHandler searchHandler,
        GetProductsByCategoryQueryHandler categoryHandler,
        CancellationToken cancellationToken)
    {
        var validation = CatalogRequestValidation.ValidateListQuery(search, category, cursor, limit);
        if (!validation.IsValid)
        {
            return TypedResults.ValidationProblem(validation.Errors);
        }

        if (!string.IsNullOrWhiteSpace(category))
        {
            var result = await categoryHandler.HandleAsync(
                new GetProductsByCategoryQuery(category, search, cursor, limit),
                cancellationToken);
            return TypedResults.Ok<object>(result);
        }

        if (!string.IsNullOrWhiteSpace(search))
        {
            var result = await searchHandler.HandleAsync(
                new SearchProductsQuery(search, cursor, limit),
                cancellationToken);
            return TypedResults.Ok<object>(result);
        }

        var browseResult = await browseHandler.HandleAsync(
            new BrowseProductsQuery(cursor, limit),
            cancellationToken);
        return TypedResults.Ok<object>(browseResult);
    }

    private static async Task<Results<Ok<object>, NotFound, ValidationProblem>> GetProductDetailAsync(
        string productSlug,
        GetProductDetailQueryHandler handler,
        CancellationToken cancellationToken)
    {
        var validation = CatalogRequestValidation.ValidateProductSlug(productSlug);
        if (!validation.IsValid)
        {
            return TypedResults.ValidationProblem(validation.Errors);
        }

        var result = await handler.HandleAsync(new GetProductDetailQuery(productSlug), cancellationToken);
        return result is null ? TypedResults.NotFound() : TypedResults.Ok<object>(result);
    }

    private static async Task<Ok<object>> GetCategoriesAsync(
        GetCatalogCategoriesQueryHandler handler,
        CancellationToken cancellationToken)
    {
        var result = await handler.HandleAsync(new GetCatalogCategoriesQuery(), cancellationToken);
        return TypedResults.Ok<object>(result);
    }
}
