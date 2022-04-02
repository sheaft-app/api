namespace Sheaft.Domain.ProductManagement;

public class Product : AggregateRoot
{
    private Product()
    {
    }

    public Product(ProductName name, ProductCode code, VatRate vat, string? description, SupplierId supplierIdentifier)
    {
        Identifier = ProductId.New();
        Name = name;
        Code = code;
        Description = description;
        SupplierIdentifier = supplierIdentifier;
        Vat = vat;
    }

    public ProductId Identifier { get; }
    public ProductName Name { get; private set; }
    public ProductCode Code { get; private set; }
    public string? Description { get; private set; }
    public SupplierId SupplierIdentifier { get; private set; }
    public VatRate Vat { get; private set; }

    public Result UpdateInfo(ProductName name, VatRate vat, string? description)
    {
        Name = name;
        Description = description;
        Vat = vat;
        return Result.Success();
    }

    public Result UpdateCode(ProductCode code)
    {
        Code = code;
        return Result.Success();
    }
}