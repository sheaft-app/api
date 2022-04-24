using Sheaft.Domain;
using Sheaft.Domain.DocumentManagement;

namespace Sheaft.Application.DocumentManagement;

public record RemoveDocumentCommand(DocumentId DocumentIdentifier) : Command<Result>;
    
public class RemoveDocumentHandler : ICommandHandler<RemoveDocumentCommand, Result>
{
    private readonly IUnitOfWork _uow;
    private readonly IFileStorage _fileStorage;

    public RemoveDocumentHandler(
        IUnitOfWork uow,
        IFileStorage fileStorage)
    {
        _uow = uow;
        _fileStorage = fileStorage;
    }

    public async Task<Result> Handle(RemoveDocumentCommand request, CancellationToken token)
    {
        var documentResult = await _uow.Documents.Get(request.DocumentIdentifier, token);
        if (documentResult.IsFailure)
            return Result.Failure<string>(documentResult);

        var document = documentResult.Value;
        var removeResult = await _fileStorage.RemoveDocument(document, token);
        if (removeResult.IsFailure)
            return removeResult;
        
        _uow.Documents.Remove(documentResult.Value);
        return await _uow.Save(token);
    }
}