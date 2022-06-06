namespace Sheaft.Domain.ProductManagement;

public class Product : AggregateRoot
{
    private Product()
    {
    }

    public Product(ProductName name, ProductReference reference, VatRate vat, string? description, SupplierId supplierId, Maybe<Returnable> returnable)
    {
        Id = ProductId.New();
        Name = name;
        Reference = reference;
        Description = description;
        SupplierId = supplierId;
        Vat = vat;
        Returnable = returnable.HasValue ? returnable.Value : null;
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

    public void UpdateInfo(ProductReference reference, ProductName name, VatRate vat, string? description)
    {
        Reference = reference;
        Name = name;
        Description = description;
        Vat = vat;
        UpdatedOn = DateTimeOffset.UtcNow;
    }

    public void SetReturnable(Maybe<Returnable> returnable)
    {
        if (Returnable?.Id != null && returnable.HasNoValue)
            Returnable = null;
        else if (returnable.HasValue)
            Returnable = returnable.Value;
        
        UpdatedOn = DateTimeOffset.UtcNow;
    }
}