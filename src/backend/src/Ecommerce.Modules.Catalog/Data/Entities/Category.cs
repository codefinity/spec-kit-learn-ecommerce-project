namespace Ecommerce.Modules.Catalog.Data.Entities;

public sealed class Category
{
    public Guid Id { get; init; }
    public string Slug { get; init; } = "";
    public string Name { get; init; } = "";
    public bool IsBrowseable { get; init; }
}
