namespace Sheaft.Domain.ProductManagement;

public class Product : AggregateRoot
{
    private Product()
    {
    }

    public Product(ProductName name, ProductReference reference, VatRate vat, string? description, SupplierId supplierId, Returnable? returnable = null)
    {
        Id = ProductId.New();
        Name = name;
        Reference = reference;
        Description = description;
        SupplierId = supplierId;
        Vat = vat;
        Returnable = returnable;
        CreatedOn = DateTimeOffset.UtcNow;
        UpdatedOn = DateTimeOffset.UtcNow;
    }

    public ProductId Id { get; }
    public ProductName Name { get; private set; }
    public ProductReference Reference { get; private set; }
    public string? Description { get; private set; }
    public SupplierId SupplierId { get; }
    public VatRate Vat { get; private set; }
    public Returnable? Returnable { get; private set; }
    public DateTimeOffset CreatedOn { get; private set; }
    public DateTimeOffset UpdatedOn { get; private set; }

    public Result UpdateInfo(ProductName name, VatRate vat, string? description)
    {
        Name = name;
        Description = description;
        Vat = vat;
        UpdatedOn = DateTimeOffset.UtcNow;
        return Result.Success();
    }

    public Result UpdateCode(ProductReference reference)
    {
        Reference = reference;
        UpdatedOn = DateTimeOffset.UtcNow;
        return Result.Success();
    }

    public Result SetReturnable(Returnable? returnable)
    {
        Returnable = returnable;
        UpdatedOn = DateTimeOffset.UtcNow;
        return Result.Success();
    }
}