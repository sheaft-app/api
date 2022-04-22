using Sheaft.Domain;
using Sheaft.Domain.DocumentManagement;

namespace Sheaft.Application.DocumentManagement;

public record DownloadDocumentCommand(DocumentId DocumentIdentifier) : Command<Result<string>>;
    
public class DownloadDocumentHandler : ICommandHandler<DownloadDocumentCommand, Result<string>>
{
    private readonly IUnitOfWork _uow;
    private readonly IFileStorage _fileStorage;

    public DownloadDocumentHandler(
        IUnitOfWork uow,
        IFileStorage fileStorage)
    {
        _uow = uow;
        _fileStorage = fileStorage;
    }

    public async Task<Result<string>> Handle(DownloadDocumentCommand request, CancellationToken token)
    {
        var documentResult = await _uow.Documents.Get(request.DocumentIdentifier, token);
        if (documentResult.IsFailure)
            return Result.Failure<string>(documentResult);

        var document = documentResult.Value;
        return await _fileStorage.DownloadDocument(document, token);
    }
}