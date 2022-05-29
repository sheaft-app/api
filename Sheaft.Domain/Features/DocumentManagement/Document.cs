namespace Sheaft.Domain.DocumentManagement;

public class Document : AggregateRoot
{
    private string _params;

    private Document()
    {
    }

    private Document(OwnerId ownerId, DocumentCategory category, DocumentKind kind, DocumentExtension extension, DocumentName name, string data)
    {
        Id = DocumentId.New();
        Category = category;
        Kind = kind;
        Status = DocumentStatus.Waiting;
        OwnerId = ownerId;
        Extension = extension;
        Name = name;
        _params = data;
        CreatedOn = DateTimeOffset.UtcNow;
        UpdatedOn = DateTimeOffset.UtcNow;
    }

    public static Document CreatePreparationDocument(DocumentName name, IDocumentParamsHandler documentParamsHandler,
        List<OrderId> orderIdentifiers, OwnerId ownerId)
    {
        return new Document(ownerId, DocumentCategory.Orders, DocumentKind.Preparation, DocumentExtension.xlsx, name,
            documentParamsHandler.SerializeParams(new PreparationDocumentParams(orderIdentifiers)));
    }

    public DocumentId Id { get; }
    public DocumentCategory Category { get; }
    public DocumentKind Kind { get; }
    public DocumentExtension Extension { get; set; }
    public DocumentStatus Status { get; private set; }
    public DocumentName Name { get; private set; }
    public OwnerId OwnerId { get; set; }
    public string? ErrorMessage { get; private set; }
    public DateTimeOffset CreatedOn { get; private set; }
    public DateTimeOffset UpdatedOn { get; private set; }

    public T GetParams<T>(IDocumentParamsHandler documentParamsHandler) where T: class
    {
        return documentParamsHandler.GetParams<T>(_params);
    }

    public Result StartProcessing()
    {
        Status = DocumentStatus.Processing;
        UpdatedOn = DateTimeOffset.UtcNow;
        return Result.Success();
    }

    public Result CompleteProcessing()
    {
        Status = DocumentStatus.Done;
        UpdatedOn = DateTimeOffset.UtcNow;
        return Result.Success();
    }

    public void SetProcessingError(string message)
    {
        Status = DocumentStatus.InError;
        UpdatedOn = DateTimeOffset.UtcNow;
        ErrorMessage = message;
    }
}

public record PreparationDocumentParams(IEnumerable<OrderId> OrderIdentifiers);