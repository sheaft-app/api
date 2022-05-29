using Sheaft.Domain;
using Sheaft.Domain.DocumentManagement;

namespace Sheaft.Application.DocumentManagement;

public record ProcessDocumentCommand(DocumentId DocumentIdentifier) : Command<Result>;
    
public class ProcessDocumentHandler : ICommandHandler<ProcessDocumentCommand, Result>
{
    private readonly IUnitOfWork _uow;
    private readonly IDocumentProcessorFactory _documentProcessorFactory;

    public ProcessDocumentHandler(
        IUnitOfWork uow,
        IDocumentProcessorFactory documentProcessorFactory)
    {
        _uow = uow;
        _documentProcessorFactory = documentProcessorFactory;
    }

    public async Task<Result> Handle(ProcessDocumentCommand request, CancellationToken token)
    {
        var documentResult = await _uow.Documents.Get(request.DocumentIdentifier, token);
        if (documentResult.IsFailure)
            return documentResult;

        var document = documentResult.Value;
        var documentProcessor = _documentProcessorFactory.GetProcessor(document);
        
        var result = await documentProcessor.Process(document.Id, token);
        if (!result.IsFailure)
            return result;
        
        return await _uow.Save(token);
    }
}