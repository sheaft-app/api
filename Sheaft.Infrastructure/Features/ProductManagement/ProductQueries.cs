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
                    Returnable = r.Id != null ? new ReturnableDto
                    {
                        Id = r.Id,
                        Code = r.Reference,
                        Name = r.Name,
                        Vat = r.Vat,
                        UnitPrice = r.UnitPrice,
                        CreatedOn = r.CreatedOn,
                        UpdatedOn = r.UpdatedOn
                    } : null
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
                where p.SupplierId == supplierId.Value && c.SupplierId == supplierId.Value
                select new ProductDto
                {
                    Id = p.Id,
                    Name = p.Name,
                    Code = p.Reference,
                    Vat = p.Vat,
                    UnitPrice = cp.UnitPrice,
                    CreatedOn = p.CreatedOn,
                    UpdatedOn = p.UpdatedOn,
                };

            var products = await productsQuery
                .Skip(pageInfo.Skip)
                .Take(pageInfo.Take)
                .GroupBy(p => new { Total = productsQuery.Count() })
                .FirstOrDefaultAsync(token);
            
            return Result.Success(new PagedResult<ProductDto>(products?.Select(p => p), pageInfo, products?.Key.Total ?? 0));
        });
    }
}