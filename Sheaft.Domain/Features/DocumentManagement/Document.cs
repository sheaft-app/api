namespace Sheaft.Domain.DocumentManagement;

public class Document : AggregateRoot
{
    private string _params;

    private Document()
    {
    }

    private Document(SupplierId supplierIdentifier, DocumentCategory category, DocumentKind kind, string name, string data)
    {
        Identifier = DocumentId.New();
        Category = category;
        Kind = kind;
        Status = DocumentStatus.Waiting;
        SupplierIdentifier = supplierIdentifier;
        Name = name;
        _params = data;
    }

    public static Document CreatePreparationDocument(string name, IDocumentParamsHandler documentParamsHandler,
        List<OrderId> orderIdentifiers, SupplierId supplierIdentifier)
    {
        return new Document(supplierIdentifier, DocumentCategory.Orders, DocumentKind.Preparation, name,
            documentParamsHandler.SerializeParams(new PreparationDocumentParams(orderIdentifiers)));
    }

    public DocumentId Identifier { get; }
    public DocumentCategory Category { get; }
    public DocumentKind Kind { get; }
    public DocumentStatus Status { get; private set; }
    public string Name { get; private set; }
    public string? Url { get; private set; }
    public SupplierId SupplierIdentifier { get; set; }
    public string? ErrorMessage { get; private set; }

    public T GetParams<T>(IDocumentParamsHandler documentParamsHandler) where T: class
    {
        return documentParamsHandler.GetParams<T>(_params);
    }

    public Result StartProcessing()
    {
        Status = DocumentStatus.Processing;
        return Result.Success();
    }

    public Result CompleteProcessing(string url)
    {
        Url = url;
        Status = DocumentStatus.Done;
        return Result.Success();
    }

    public void SetProcessingError(string message)
    {
        Status = DocumentStatus.InError;
        ErrorMessage = message;
    }
}

public record PreparationDocumentParams(IEnumerable<OrderId> OrderIdentifiers);