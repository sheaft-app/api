namespace Sheaft.Domain.ProductManagement;

public class CatalogProduct
{
    private CatalogProduct(){}
    
    public CatalogProduct(Product product, ProductUnitPrice unitPrice)
    {
        Product = product;
        SetPrice(unitPrice);
    }

    public Product Product { get; private set; }
    public ProductUnitPrice UnitPrice { get; private set; }

    public Result SetPrice(ProductUnitPrice newUnitPrice)
    {
        UnitPrice = newUnitPrice;
        return Result.Success();
    }
}