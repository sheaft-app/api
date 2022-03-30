namespace Sheaft.Domain.ProductManagement;

public class Product : AggregateRoot
{
    private Product()
    {
    }

    public Product(ProductName name, ProductCode code, string? description, SupplierId supplierIdentifier)
    {
        Identifier = ProductId.New();
        Name = name;
        Code = code;
        Description = description;
        SupplierIdentifier = supplierIdentifier;
    }

    public ProductId Identifier { get; }
    public ProductName Name { get; private set; }
    public ProductCode Code { get; private set; }
    public string? Description { get; private set; }
    public SupplierId SupplierIdentifier { get; private set; }
}