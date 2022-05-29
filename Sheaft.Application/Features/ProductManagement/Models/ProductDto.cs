namespace Sheaft.Application.ProductManagement;

public record ProductDto
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string Reference { get; set; }
    public string? Description { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal Vat { get; set; }
}