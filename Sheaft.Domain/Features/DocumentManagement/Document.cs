﻿namespace Sheaft.Domain.DocumentManagement;

public class Document : AggregateRoot
{
    private string _params;

    private Document()
    {
    }

    private Document(SupplierId supplierIdentifier, DocumentCategory category, DocumentKind kind, DocumentExtension extension, DocumentName name, string data)
    {
        Identifier = DocumentId.New();
        Category = category;
        Kind = kind;
        Status = DocumentStatus.Waiting;
        SupplierIdentifier = supplierIdentifier;
        Extension = extension;
        Name = name;
        _params = data;
    }

    public static Document CreatePreparationDocument(DocumentName name, IDocumentParamsHandler documentParamsHandler,
        List<OrderId> orderIdentifiers, SupplierId supplierIdentifier)
    {
        return new Document(supplierIdentifier, DocumentCategory.Orders, DocumentKind.Preparation, DocumentExtension.xlsx, name,
            documentParamsHandler.SerializeParams(new PreparationDocumentParams(orderIdentifiers)));
    }

    public DocumentId Identifier { get; }
    public DocumentCategory Category { get; }
    public DocumentKind Kind { get; }
    public DocumentExtension Extension { get; set; }
    public DocumentStatus Status { get; private set; }
    public DocumentName Name { get; private set; }
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

    public Result CompleteProcessing()
    {
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