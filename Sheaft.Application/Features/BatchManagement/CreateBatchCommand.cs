using Sheaft.Domain;
using Sheaft.Domain.BatchManagement;
using Sheaft.Domain.ProductManagement;

namespace Sheaft.Application.BatchManagement;

public record CreateBatchCommand(string Number, BatchDateKind DateKind, DateOnly Date, SupplierId SupplierIdentifier) : ICommand<Result<string>>;

internal class CreateBatchHandler : ICommandHandler<CreateBatchCommand, Result<string>>
{
    private readonly IUnitOfWork _uow;

    public CreateBatchHandler(IUnitOfWork uow)
    {
        _uow = uow;
    }
    
    public async Task<Result<string>> Handle(CreateBatchCommand request, CancellationToken token)
    {
        var batch = new Batch(new BatchNumber(request.Number), request.DateKind, request.Date, request.SupplierIdentifier);
        _uow.Batches.Add(batch);
        var result = await _uow.Save(token);
        
        return result.IsSuccess 
            ? Result.Success(batch.Identifier.Value) 
            : Result.Failure<string>(result);
    }
}