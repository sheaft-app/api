﻿using Sheaft.Domain;
using Sheaft.Domain.BatchManagement;

namespace Sheaft.Application.BatchManagement;

public record CreateBatchCommand(string Number, BatchDateKind DateKind, DateTime ExpirationDate, DateTime? ProductionDate, SupplierId SupplierIdentifier) : Command<Result<string>>;

internal class CreateBatchHandler : ICommandHandler<CreateBatchCommand, Result<string>>
{
    private readonly IUnitOfWork _uow;

    public CreateBatchHandler(IUnitOfWork uow)
    {
        _uow = uow;
    }
    
    public async Task<Result<string>> Handle(CreateBatchCommand request, CancellationToken token)
    {
        var existingBatchResult = await _uow.Batches.Find(new BatchNumber(request.Number), token);
        if (existingBatchResult.IsFailure)
            return Result.Failure<string>(existingBatchResult);
        
        if (existingBatchResult.Value.HasValue)
            return Result.Failure<string>(ErrorKind.Conflict, "batch.number.already.exists");
        
        var batch = new Batch(new BatchNumber(request.Number), request.DateKind, request.ExpirationDate, request.ProductionDate, request.SupplierIdentifier);
        _uow.Batches.Add(batch);
        var result = await _uow.Save(token);
        
        return result.IsSuccess 
            ? Result.Success(batch.Id.Value) 
            : Result.Failure<string>(result);
    }
}