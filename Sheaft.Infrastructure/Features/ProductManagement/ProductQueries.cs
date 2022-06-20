using DataModel;
using LinqToDB;
using Sheaft.Application;
using Sheaft.Application.ProductManagement;
using Sheaft.Domain;

namespace Sheaft.Infrastructure.ProductManagement;

internal class ProductQueries : Queries, IProductQueries
{
    private readonly AppDb _context;

    public ProductQueries(AppDb context)
    {
        _context = context;
    }
    
    public Task<Result<ProductDetailsDto>> Get(ProductId identifier, CancellationToken token)
    {
        return QueryAsync(async () =>
        {
            var productsQuery =
                from p in _context.Products
                from cp in _context.CatalogProducts.Where(cp => cp.ProductId == p.Id)
                from c in _context.Catalogs.Where(c => c.Id == cp.CatalogId && c.IsDefault)
                from r in _context.Returnables.Where(r => r.Id == p.ReturnableId).DefaultIfEmpty()
                where p.Id == identifier.Value
                select new ProductDetailsDto
                {
                    Id = p.Id,
                    Name = p.Name,
                    Description = p.Description,
                    Code = p.Reference,
                    Vat = p.Vat,
                    UnitPrice = cp.UnitPrice,
                    CreatedOn = p.CreatedOn,
                    UpdatedOn = p.UpdatedOn,
                    ReturnableId = r.Id,
                    Returnable = r.Id != null ? new ReturnableDto(r.Id, r.Name, r.Reference, r.UnitPrice, r.Vat, r.CreatedOn, r.UpdatedOn) : null
                };

            var product = await productsQuery.FirstOrDefaultAsync(token);
            return product == null
                ? Result.Failure<ProductDetailsDto>(ErrorKind.NotFound, "product.notfound")
                : Result.Success(product);
        });
    }

    public Task<Result<PagedResult<ProductDto>>> List(SupplierId supplierId, PageInfo pageInfo, CancellationToken token)
    {
        return QueryAsync(async () =>
        {
            var productsQuery =
                from p in _context.Products
                from cp in _context.CatalogProducts.Where(cp => cp.ProductId == p.Id)
                from c in _context.Catalogs.Where(c => c.Id == cp.CatalogId && c.IsDefault)
                orderby p.Name 
                where p.SupplierId == supplierId.Value && c.SupplierId == supplierId.Value
                select new 
                {
                    Id = p.Id,
                    Name = p.Name,
                    Code = p.Reference,
                    Vat = p.Vat,
                    UnitPrice = cp.UnitPrice,
                    UpdatedOn = p.UpdatedOn,
                    TotalCount = Sql.Ext.Count().Over().ToValue()
                };

            var products = (await productsQuery
                .Skip(pageInfo.Skip)
                .Take(pageInfo.Take)
                .ToListAsync(token))
                .GroupBy(p => p.TotalCount)
                .FirstOrDefault();
            
            return Result.Success(new PagedResult<ProductDto>(
                products?.Select(p => 
                    new ProductDto(p.Id, p.Name, p.Code, p.UnitPrice, p.Vat, p.UpdatedOn)), 
                pageInfo, products?.Key ?? 0));
        });
    }

    public Task<Result<PagedResult<OrderableProductDto>>> ListOrderable(AccountId customerAccountId, SupplierId supplierId, PageInfo pageInfo, CancellationToken token)
    {
        return QueryAsync(async () =>
        {
            var productsQuery =
                from agreement in _context.Agreements
                from supplier in _context.Suppliers.Where(s => s.Id == agreement.SupplierId)
                from catalog in _context.Catalogs.Where(c => c.Id == agreement.CatalogId)
                from catalogProduct in _context.CatalogProducts.Where(cp => cp.CatalogId == catalog.Id)
                from product in _context.Products.Where(p => p.Id == catalogProduct.ProductId)
                where agreement.Customer.AccountId == customerAccountId.Value 
                      && (AgreementStatus)agreement.Status == AgreementStatus.Active 
                      && agreement.SupplierId == supplierId.Value
                orderby product.Name
                select new 
                {
                    Id = product.Id,
                    Name = product.Name,
                    Code = product.Reference,
                    Vat = product.Vat,
                    UnitPrice = catalogProduct.UnitPrice,
                    UpdatedOn = product.UpdatedOn,
                    SupplierName = supplier.TradeName,
                    SupplierId = supplier.Id,
                    TotalCount = Sql.Ext.Count().Over().ToValue()
                };

            var products = (await productsQuery
                    .Skip(pageInfo.Skip)
                    .Take(pageInfo.Take)
                    .ToListAsync(token))
                .GroupBy(p => p.TotalCount)
                .FirstOrDefault();
            
            return Result.Success(new PagedResult<OrderableProductDto>(
                products?.Select(p => 
                    new OrderableProductDto(p.Id, p.Name, p.Code, p.UnitPrice, p.Vat, p.UpdatedOn, p.SupplierName, p.SupplierId)), 
                pageInfo, products?.Key ?? 0));
        });
    }
}