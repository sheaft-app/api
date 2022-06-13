namespace Sheaft.Application.ProductManagement;

public record ProductDetailsDto
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string Code { get; set; }
    public string? Description { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal Vat { get; set; }
    public ReturnableDto? Returnable { get; set; }
    public string? ReturnableId { get; set; }
    public DateTimeOffset CreatedOn { get; set; }
    public DateTimeOffset UpdatedOn { get; set; }
}