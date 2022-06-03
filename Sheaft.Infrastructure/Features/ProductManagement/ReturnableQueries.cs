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
                select new ReturnableDto
                {
                    Id = p.Id,
                    Name = p.Name,
                    Code = p.Reference,
                    Vat = p.Vat,
                    UnitPrice = p.UnitPrice
                };

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
                where p.SupplierId == supplierId.Value
                select new ReturnableDto
                {
                    Id = p.Id,
                    Name = p.Name,
                    Code = p.Reference,
                    Vat = p.Vat,
                    UnitPrice = p.UnitPrice
                };

            var returnables = await returnablesQuery
                .Skip(pageInfo.Skip)
                .Take(pageInfo.Take)
                .GroupBy(p => new { Total = returnablesQuery.Count() })
                .FirstOrDefaultAsync(token);
            
            return Result.Success(new PagedResult<ReturnableDto>(returnables?.Select(p => p), pageInfo, returnables?.Key.Total ?? 0));
        });
    }
}