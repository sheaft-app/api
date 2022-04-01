namespace Sheaft.Domain.ProductManagement;

public class CatalogProduct
{
    private CatalogProduct(){}
    
    public CatalogProduct(Product product, ProductPrice price)
    {
        Product = product;
        SetPrice(price);
    }

    public Product Product { get; private set; }
    public ProductPrice Price { get; private set; }

    public Result SetPrice(ProductPrice newPrice)
    {
        Price = newPrice;
        return Result.Success();
    }
}