﻿namespace Sheaft.Domain.ProductManagement;

public class CatalogProduct
{
    private CatalogProduct(){}
    
    internal CatalogProduct(CatalogId catalogId, Product product, ProductUnitPrice unitPrice)
    {
        CatalogId = catalogId;
        Product = product;
        SetPrice(unitPrice);
        CreatedOn = DateTimeOffset.UtcNow;
        UpdatedOn = DateTimeOffset.UtcNow;
    }

    public CatalogId CatalogId { get; }
    public Product Product { get; private set; }
    public ProductUnitPrice UnitPrice { get; private set; }
    public DateTimeOffset CreatedOn { get; private set; }
    public DateTimeOffset UpdatedOn { get; private set; }

    public Result SetPrice(ProductUnitPrice newUnitPrice)
    {
        UnitPrice = newUnitPrice;
        UpdatedOn = DateTimeOffset.UtcNow;
        return Result.Success();
    }
}