using Sheaft.Domain;
using Sheaft.Domain.DocumentManagement;

namespace Sheaft.Application.DocumentManagement;

public record CreatePreparationDocumentCommand(List<OrderId> OrderIdentifiers, SupplierId SupplierIdentifier, bool AcceptPendingOrders = true) : Command<Result<string>>;
    
public class CreatePreparationDocumentHandler : ICommandHandler<CreatePreparationDocumentCommand, Result<string>>
{
    private readonly IUnitOfWork _uow;
    private readonly IDocumentParamsHandler _documentParamsHandler;

    public CreatePreparationDocumentHandler(
        IUnitOfWork uow,
        IDocumentParamsHandler documentParamsHandler)
    {
        _uow = uow;
        _documentParamsHandler = documentParamsHandler;
    }

    public async Task<Result<string>> Handle(CreatePreparationDocumentCommand request, CancellationToken token)
    {
        var document = Document.CreatePreparationDocument($"Préparation du {request.CreatedAt:d}", _documentParamsHandler, request.OrderIdentifiers, request.SupplierIdentifier);
        _uow.Documents.Add(document);
        
        var result = await _uow.Save(token);
        if (result.IsFailure)
            return Result.Failure<string>(result);

        return Result.Success(document.Identifier.Value);
    }
}