using Sheaft.Domain;
using Sheaft.Domain.BatchManagement;

namespace Sheaft.Application.BatchManagement;

public record RemoveBatchCommand(BatchId BatchIdentifier) : ICommand<Result>;

internal class RemoveBatchHandler : ICommandHandler<RemoveBatchCommand, Result>
{
    private readonly IUnitOfWork _uow;
    private readonly IValidateAlteringBatchCapability _validateAlteringBatchCapability;

    public RemoveBatchHandler(
        IUnitOfWork uow,
        IValidateAlteringBatchCapability validateAlteringBatchCapability)
    {
        _uow = uow;
        _validateAlteringBatchCapability = validateAlteringBatchCapability;
    }
    
    public async Task<Result> Handle(RemoveBatchCommand request, CancellationToken token)
    {
        var canAlterBatchResult = await _validateAlteringBatchCapability.CanAlterBatch(request.BatchIdentifier, token);
        if (canAlterBatchResult.IsFailure)
            return canAlterBatchResult;

        if (!canAlterBatchResult.Value)
            return Result.Failure(ErrorKind.BadRequest, "batch.cannot.remove.already.in.use");

        var batchResult = await _uow.Batches.Get(request.BatchIdentifier, token);
        if (batchResult.IsFailure)
            return batchResult;
        
        _uow.Batches.Remove(batchResult.Value);
        return await _uow.Save(token);
    }
}