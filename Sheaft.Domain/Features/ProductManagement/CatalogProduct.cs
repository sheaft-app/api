namespace Sheaft.Domain.ProductManagement;

public class CatalogProduct
{
    private CatalogProduct(){}
    
    public CatalogProduct(Product product, int price)
    {
        Product = product;
        SetPrice(price);
    }

    public Product Product { get; private set; }
    public int Price { get; private set; }

    public void SetPrice(int newPrice)
    {
        if (newPrice <= 0)
            throw new InvalidOperationException("Catalog Product new price must be greater than 0");
        
        Price = newPrice;
    }
}