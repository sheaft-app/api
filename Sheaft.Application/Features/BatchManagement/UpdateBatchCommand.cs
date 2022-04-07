using Sheaft.Domain;
using Sheaft.Domain.BatchManagement;
using Sheaft.Domain.ProductManagement;

namespace Sheaft.Application.BatchManagement;

public record UpdateBatchCommand(BatchId BatchIdentifier, string Number, BatchDateKind DateKind, DateOnly Date) : ICommand<Result>;

internal class UpdateBatchHandler : ICommandHandler<UpdateBatchCommand, Result>
{
    private readonly IUnitOfWork _uow;
    private readonly IValidateAlteringBatchFeasability _validateAlteringBatchFeasability;

    public UpdateBatchHandler(
        IUnitOfWork uow,
        IValidateAlteringBatchFeasability validateAlteringBatchFeasability)
    {
        _uow = uow;
        _validateAlteringBatchFeasability = validateAlteringBatchFeasability;
    }
    
    public async Task<Result> Handle(UpdateBatchCommand request, CancellationToken token)
    {
        var canAlterBatchResult = await _validateAlteringBatchFeasability.CanAlterBatch(request.BatchIdentifier, token);
        if (canAlterBatchResult.IsFailure)
            return canAlterBatchResult;

        if (!canAlterBatchResult.Value)
            return Result.Failure(ErrorKind.BadRequest, "batch.cannot.update.already.in.use");

        var batchResult = await _uow.Batches.Get(request.BatchIdentifier, token);
        if (batchResult.IsFailure)
            return batchResult;

        var batch = batchResult.Value;
        batch.Update(new BatchNumber(request.Number), request.DateKind, request.Date);
        
        _uow.Batches.Update(batch);
        return await _uow.Save(token);
    }
}