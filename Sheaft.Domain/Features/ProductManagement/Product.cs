namespace Sheaft.Domain.ProductManagement;

public class Product : AggregateRoot
{
    private Product()
    {
    }

    public Product(ProductName name, ProductReference reference, VatRate vat, string? description, SupplierId supplierIdentifier, Returnable? returnable = null)
    {
        Identifier = ProductId.New();
        Name = name;
        Reference = reference;
        Description = description;
        SupplierIdentifier = supplierIdentifier;
        Vat = vat;
        Returnable = returnable;
    }

    public ProductId Identifier { get; }
    public ProductName Name { get; private set; }
    public ProductReference Reference { get; private set; }
    public string? Description { get; private set; }
    public SupplierId SupplierIdentifier { get; }
    public VatRate Vat { get; private set; }
    public Returnable? Returnable { get; private set; }

    public Result UpdateInfo(ProductName name, VatRate vat, string? description)
    {
        Name = name;
        Description = description;
        Vat = vat;
        return Result.Success();
    }

    public Result UpdateCode(ProductReference reference)
    {
        Reference = reference;
        return Result.Success();
    }

    public Result SetReturnable(Returnable? returnable)
    {
        Returnable = returnable;
        return Result.Success();
    }
}