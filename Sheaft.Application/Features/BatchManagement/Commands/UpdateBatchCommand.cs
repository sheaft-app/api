using Sheaft.Domain;
using Sheaft.Domain.BatchManagement;

namespace Sheaft.Application.BatchManagement;

public record UpdateBatchCommand(SupplierId SupplierId, BatchId BatchIdentifier, string Number,
    BatchDateKind DateKind, DateTime ExpirationDate, DateTime? ProductionDate) : Command<Result>;

internal class UpdateBatchHandler : ICommandHandler<UpdateBatchCommand, Result>
{
    private readonly IUnitOfWork _uow;
    private readonly IValidateAlteringBatchCapability _validateAlteringBatchCapability;

    public UpdateBatchHandler(
        IUnitOfWork uow,
        IValidateAlteringBatchCapability validateAlteringBatchCapability)
    {
        _uow = uow;
        _validateAlteringBatchCapability = validateAlteringBatchCapability;
    }
    
    public async Task<Result> Handle(UpdateBatchCommand request, CancellationToken token)
    {
        var canAlterBatchResult = await _validateAlteringBatchCapability.CanAlterBatch(request.BatchIdentifier, token);
        if (canAlterBatchResult.IsFailure)
            return canAlterBatchResult;

        if (!canAlterBatchResult.Value)
            return Result.Failure(ErrorKind.BadRequest, "batch.cannot.update.already.in.use");

        var batchResult = await _uow.Batches.Get(request.BatchIdentifier, token);
        if (batchResult.IsFailure)
            return batchResult;
        
        var batch = batchResult.Value;

        if (request.Number != batch.Number.Value)
        {
            var batchNumber = await _uow.Batches.Find(new BatchNumber(request.Number), token);
            if (batchNumber.IsFailure)
                return batchNumber;

            if (batchNumber.Value.HasValue && batchNumber.Value.Value.Id != batch.Id)
                return Result.Failure(ErrorKind.Conflict, "batch.number.already.exists");
        }
        
        batch.Update(new BatchNumber(request.Number), request.DateKind, request.ExpirationDate, request.ProductionDate);
        
        _uow.Batches.Update(batch);
        return await _uow.Save(token);
    }
}