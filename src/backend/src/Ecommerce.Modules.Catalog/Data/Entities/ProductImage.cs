namespace Ecommerce.Modules.Catalog.Data.Entities;

public sealed class ProductImage
{
    public Guid Id { get; init; }
    public Guid ProductId { get; init; }
    public string Url { get; init; } = "";
    public string AltText { get; init; } = "";
    public int SortOrder { get; init; }
    public bool IsPrimary { get; init; }
}
