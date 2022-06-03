namespace Sheaft.Domain.ProductManagement;

public interface IHandleReturnableCode
{
    Task<Result<ReturnableReference>> ValidateOrGenerateNextCodeForReturnable(string? code, ReturnableId returnableId, 
        SupplierId supplierIdentifier,
        CancellationToken token);
    
    Task<Result<ReturnableReference>> ValidateOrGenerateNextCode(string? code, 
        SupplierId supplierIdentifier,
        CancellationToken token);
}

internal class HandleReturnableCode : IHandleReturnableCode
{
    private readonly IReturnableRepository _returnableRepository;
    private readonly IGenerateReturnableCode _generateReturnableCode;

    public HandleReturnableCode(
        IReturnableRepository returnableRepository,
        IGenerateReturnableCode generateReturnableCode)
    {
        _returnableRepository = returnableRepository;
        _generateReturnableCode = generateReturnableCode;
    }

    public async Task<Result<ReturnableReference>> ValidateOrGenerateNextCodeForReturnable(string? code, ReturnableId returnableId, 
        SupplierId supplierIdentifier, CancellationToken token)
    {
        return await GenerateNextReturnableCode(code, returnableId, supplierIdentifier, token);
    }

    public async Task<Result<ReturnableReference>> ValidateOrGenerateNextCode(string? code, 
        SupplierId supplierIdentifier, CancellationToken token)
    {
        return await GenerateNextReturnableCode(code, null, supplierIdentifier, token);
    }

    private async Task<Result<ReturnableReference>> GenerateNextReturnableCode(string? code, ReturnableId? returnableId, SupplierId supplierIdentifier, CancellationToken token)
    {
        if (string.IsNullOrWhiteSpace(code))
            return _generateReturnableCode.GenerateNextCode(supplierIdentifier);

        var returnableReference = new ReturnableReference(code.Replace(" ", "-").ToUpperInvariant());
        var existingReturnableResult = await _returnableRepository.Find(returnableReference, supplierIdentifier, token);
        if (existingReturnableResult.IsFailure)
            return Result.Failure<ReturnableReference>(existingReturnableResult);

        if (existingReturnableResult.Value.HasNoValue)
            return Result.Success(returnableReference);

        if(returnableId != null && existingReturnableResult.Value.Value.Id == returnableId)
            return Result.Success(returnableReference);

        return Result.Failure<ReturnableReference>(ErrorKind.Conflict, "returnable.code.already.exists");
    }
}