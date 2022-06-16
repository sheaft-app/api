using DataModel;
using LinqToDB;
using Sheaft.Application;
using Sheaft.Application.ProductManagement;
using Sheaft.Domain;

namespace Sheaft.Infrastructure.ProductManagement;

internal class ReturnableQueries : Queries, IReturnableQueries
{
    private readonly AppDb _context;

    public ReturnableQueries(AppDb context)
    {
        _context = context;
    }
    
    public Task<Result<ReturnableDto>> Get(ReturnableId identifier, CancellationToken token)
    {
        return QueryAsync(async () =>
        {
            var returnablesQuery =
                from p in _context.Returnables
                where p.Id == identifier.Value
                select new ReturnableDto(p.Id, p.Name, p.Reference, p.UnitPrice, p.Vat, p.CreatedOn, p.UpdatedOn);

            var returnable = await returnablesQuery.FirstOrDefaultAsync(token);
            return returnable == null
                ? Result.Failure<ReturnableDto>(ErrorKind.NotFound, "returnable.notfound")
                : Result.Success(returnable);
        });
    }

    public Task<Result<PagedResult<ReturnableDto>>> List(SupplierId supplierId, PageInfo pageInfo, CancellationToken token)
    {
        return QueryAsync(async () =>
        {
            var returnablesQuery =
                from p in _context.Returnables
                orderby p.Name 
                where p.SupplierId == supplierId.Value
                select new 
                {
                    Id = p.Id,
                    Name = p.Name,
                    Code = p.Reference,
                    Vat = p.Vat,
                    UnitPrice = p.UnitPrice,
                    CreatedOn = p.CreatedOn,
                    UpdatedOn = p.UpdatedOn,
                    TotalCount = Sql.Ext.Count().Over().ToValue()
                };

            var returnables = (await returnablesQuery
                .Skip(pageInfo.Skip)
                .Take(pageInfo.Take)
                .ToListAsync(token))
                .GroupBy(p => p.TotalCount)
                .FirstOrDefault();
            
            return Result.Success(new PagedResult<ReturnableDto>(
                returnables?
                    .Select(p => new ReturnableDto(p.Id, p.Name, p.Code, p.UnitPrice, p.Vat, p.CreatedOn, p.UpdatedOn)), 
                pageInfo, returnables?.Key ?? 0));
        });
    }
}