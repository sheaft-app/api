using DataModel;
using LinqToDB;
using Sheaft.Application;
using Sheaft.Application.BatchManagement;
using Sheaft.Application.ProductManagement;
using Sheaft.Domain;

namespace Sheaft.Infrastructure.BatchManagement;

internal class BatchQueries : Queries, IBatchQueries
{
    private readonly AppDb _context;

    public BatchQueries(AppDb context)
    {
        _context = context;
    }
    
    public Task<Result<BatchDto>> Get(BatchId identifier, CancellationToken token)
    {
        return QueryAsync(async () =>
        {
            var batchesQuery =
                from p in _context.Batches
                where p.Id == identifier.Value
                select new BatchDto(p.Id, p.Number, (DateTimeKind)p.DateKind, DateOnly.FromDateTime(p.Date),  p.CreatedOn, p.UpdatedOn);

            var batches = await batchesQuery.FirstOrDefaultAsync(token);
            return batches== null
                ? Result.Failure<BatchDto>(ErrorKind.NotFound, "batch.notfound")
                : Result.Success(batches);
        });
    }

    public Task<Result<PagedResult<BatchDto>>> List(SupplierId supplierId, PageInfo pageInfo, CancellationToken token)
    {
        return QueryAsync(async () =>
        {
            var batchesQuery =
                from p in _context.Batches
                orderby p.Number 
                where p.SupplierId == supplierId.Value
                select new 
                {
                    Id = p.Id,
                    Number = p.Number,
                    Kind = (DateTimeKind)p.DateKind,
                    Date = p.Date,
                    CreatedOn = p.CreatedOn,
                    UpdatedOn = p.UpdatedOn,
                    TotalCount = Sql.Ext.Count().Over().ToValue()
                };

            var batches = (await batchesQuery
                .Skip(pageInfo.Skip)
                .Take(pageInfo.Take)
                .ToListAsync(token))
                .GroupBy(p => p.TotalCount)
                .FirstOrDefault();
            
            return Result.Success(new PagedResult<BatchDto>(
                batches?
                    .Select(p => new BatchDto(p.Id, p.Number, p.Kind, DateOnly.FromDateTime(p.Date), p.CreatedOn, p.UpdatedOn)), 
                pageInfo, batches?.Key ?? 0));
        });
    }
}