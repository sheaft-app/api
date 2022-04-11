using System.Collections.ObjectModel;
using Sheaft.Domain.AgreementManagement;

namespace Sheaft.Domain.InvoiceManagement;

public class Invoice : AggregateRoot
{
    private Invoice(){}
    
    private Invoice(InvoiceStatus status, SupplierId supplierIdentifier, CustomerId customerIdentifier, BillingInformation billingInformation, IEnumerable<InvoiceLine>? lines = null, InvoiceReference? reference = null)
    {
        Identifier = InvoiceId.New();
        Reference = reference;
        Status = status;
        SupplierIdentifier = supplierIdentifier;
        CustomerIdentifier = customerIdentifier;
        BillingInformation = billingInformation;
        
        SetLines(lines);
    }

    public static Invoice CreateDraft(SupplierId supplierIdentifier, CustomerId customerIdentifier, BillingInformation billingInformation)
    {
        return new Invoice(InvoiceStatus.Draft, supplierIdentifier, customerIdentifier, billingInformation);
    }

    public InvoiceId Identifier { get; }
    public InvoiceReference? Reference { get; private set; }
    public InvoiceStatus Status { get; private set; }
    public BillingInformation BillingInformation { get; private set; }
    public UnitPrice TotalPrice { get; private set; }
    public CustomerId CustomerIdentifier { get; private set; }
    public SupplierId SupplierIdentifier { get; private set; }
    public IEnumerable<InvoiceLine> Lines { get; private set; } = new List<InvoiceLine>();
    
    
    internal Result Publish(InvoiceReference reference)
    {
        if (Status != InvoiceStatus.Draft)
            return Result.Failure(ErrorKind.BadRequest, "invoice.publish.requires.draft");
        
        Reference = reference;
        Status = InvoiceStatus.Published;
        return Result.Success();
    }
    
    public Result UpdateDraftLines(IEnumerable<InvoiceLine> lines)
    {
        if (Status != InvoiceStatus.Draft)
            return Result.Failure(ErrorKind.BadRequest, "invoice.update.lines.requires.draft");
        
        SetLines(lines);
        return Result.Success();
    }

    private void SetLines(IEnumerable<InvoiceLine>? lines)
    {
        var invoiceLines = lines?.ToList() ?? new List<InvoiceLine>();
        Lines = new List<InvoiceLine>(invoiceLines);
        TotalPrice = GetTotalPrice();
    }

    private UnitPrice GetTotalPrice()
    {
        return new UnitPrice(Lines.Sum(l => l.TotalPrice.Value));
    }
}