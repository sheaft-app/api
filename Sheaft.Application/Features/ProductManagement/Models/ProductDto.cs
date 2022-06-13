namespace Sheaft.Application.ProductManagement;

public record ProductDto
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string Code { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal Vat { get; set; }
    public DateTimeOffset CreatedOn { get; set; }
    public DateTimeOffset UpdatedOn { get; set; }
}