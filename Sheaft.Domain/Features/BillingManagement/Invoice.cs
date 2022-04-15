namespace Sheaft.Domain.InvoiceManagement;

public class Invoice : AggregateRoot
{
    private Invoice()
    {
    }

    private Invoice(InvoiceKind kind, InvoiceStatus status, SupplierBillingInformation supplier,
        CustomerBillingInformation customer, IEnumerable<InvoiceLine>? lines = null,
        InvoiceReference? reference = null)
    {
        Identifier = InvoiceId.New();
        Kind = kind;
        Reference = reference;
        Status = status;
        Supplier = supplier;
        Customer = customer;

        SetLines(lines);
    }

    public static Invoice CreateInvoiceDraftForOrder(SupplierBillingInformation supplier, CustomerBillingInformation customer, IEnumerable<LockedInvoiceLine> orderLines)
    {
        return new Invoice(InvoiceKind.Invoice, InvoiceStatus.Draft, supplier, customer, orderLines);
    }

    public static Result<Invoice> CancelInvoice(Invoice invoice, InvoiceReference creditNoteReference,
        string cancellationReason, DateTimeOffset? currentDateTime = null)
    {
        var lines = invoice.Lines
            .GroupBy(l => l.Vat)
            .Select(groupedVat => new { Vat = groupedVat.Key, Price = new Price(groupedVat.Sum(e => e.PriceInfo.VatPrice.Value))})
            .Select(v => InvoiceLine.CreateLockedLine($"Avoir sur la facture n°{invoice.Reference?.Value} du {invoice.PublishedOn:d}",
                new Quantity(1), new UnitPrice(invoice.TotalWholeSalePrice.Value), v.Vat))
            .ToList();

        var creditNote = new Invoice(InvoiceKind.InvoiceCancellation, InvoiceStatus.Published, SupplierBillingInformation.Copy(invoice.Supplier),
            CustomerBillingInformation.Copy(invoice.Customer), lines, creditNoteReference);

        var result = invoice.Cancel(creditNote.Identifier, cancellationReason, currentDateTime);
        return result.IsFailure 
            ? Result.Failure<Invoice>(result) 
            : Result.Success(creditNote);
    }

    public InvoiceId Identifier { get; }
    public InvoiceReference? Reference { get; private set; }
    public InvoiceDueDate? DueOn { get; private set; }
    public InvoiceStatus Status { get; private set; }
    public InvoiceKind Kind { get; private set; }
    public DateTimeOffset? PublishedOn { get; private set; }
    public DateTimeOffset? CancelledOn { get; private set; }
    public DateTimeOffset? SentOn { get; private set; }
    public TotalWholeSalePrice TotalWholeSalePrice { get; private set; }
    public TotalVatPrice TotalVatPrice { get; private set; }
    public TotalOnSalePrice TotalOnSalePrice { get; private set; }
    public CustomerBillingInformation Customer { get; private set; }
    public SupplierBillingInformation Supplier { get; private set; }
    public IEnumerable<InvoiceLine> Lines { get; private set; } = new List<InvoiceLine>();
    public IEnumerable<InvoiceCreditNote> CreditNotes { get; private set; }
    public IEnumerable<InvoicePayment> Payments { get; private set; }
    public string? CancellationReason { get; private set; }

    internal Result Publish(InvoiceReference reference, DateTimeOffset? currentDateTime = null)
    {
        if (Status != InvoiceStatus.Draft)
            return Result.Failure(ErrorKind.BadRequest, "invoice.publish.requires.draft");
        
        if (!Lines.Any())
            return Result.Failure(ErrorKind.BadRequest, "invoice.publish.requires.lines");
        
        if (DueOn == null)
            return Result.Failure(ErrorKind.BadRequest, "invoice.publish.requires.due.date");

        Reference = reference;
        Status = InvoiceStatus.Published;
        PublishedOn = currentDateTime ?? DateTimeOffset.UtcNow;
        return Result.Success();
    }

    public Result UpdateLines(IEnumerable<InvoiceLine>? lines)
    {
        if (Status != InvoiceStatus.Draft)
            return Result.Failure(ErrorKind.BadRequest, "invoice.update.lines.requires.draft");

        SetLines(lines);
        return Result.Success();
    }

    public Result MarkAsSent(DateTimeOffset? currentDateTime = null)
    {
        if (Status != InvoiceStatus.Published)
            return Result.Failure(ErrorKind.BadRequest, "invoice.sent.requires.published");

        Status = InvoiceStatus.Sent;
        SentOn = currentDateTime ?? DateTimeOffset.UtcNow;
        return Result.Success();
    }

    public Result MarkAsPayed(string reference, PaymentKind kind, DateTimeOffset paymentDate)
    {
        if (Status != InvoiceStatus.Sent && Kind == InvoiceKind.Invoice)
            return Result.Failure(ErrorKind.BadRequest, "invoice.payed.requires.sent");

        if (Status != InvoiceStatus.Published && Status != InvoiceStatus.Sent && Kind == InvoiceKind.CreditNote)
            return Result.Failure(ErrorKind.BadRequest, "creditnote.payed.requires.published");

        Status = InvoiceStatus.Payed;
        Payments = new List<InvoicePayment> {new InvoicePayment(reference, kind, paymentDate)};
        return Result.Success();
    }

    private Result Cancel(InvoiceId creditNoteIdentifier, string cancellationReason,
        DateTimeOffset? currentDateTime = null)
    {
        if (Status != InvoiceStatus.Published && Status != InvoiceStatus.Sent)
            return Result.Failure(ErrorKind.BadRequest, "invoice.cancel.requires.published.or.sent");

        if (string.IsNullOrWhiteSpace(cancellationReason))
            return Result.Failure(ErrorKind.BadRequest, "invoice.cancel.requires.reason");

        CreditNotes = new List<InvoiceCreditNote> {new InvoiceCreditNote(creditNoteIdentifier)};
        CancellationReason = cancellationReason;
        Status = InvoiceStatus.Cancelled;
        CancelledOn = currentDateTime ?? DateTimeOffset.UtcNow;
        return Result.Success();
    }

    private void SetLines(IEnumerable<InvoiceLine>? lines)
    {
        Lines = lines?.ToList() ?? new List<InvoiceLine>();
        TotalWholeSalePrice = GetTotalWholeSalePrice();
        TotalVatPrice = GetTotalVatPrice();
        TotalOnSalePrice = GetTotalOnSalePrice();
    }

    private TotalWholeSalePrice GetTotalWholeSalePrice()
    {
        return new TotalWholeSalePrice(Lines.Sum(l => l.PriceInfo.WholeSalePrice.Value));
    }

    private TotalVatPrice GetTotalVatPrice()
    {
        return new TotalVatPrice(Lines.Sum(l => l.PriceInfo.VatPrice.Value));
    }

    private TotalOnSalePrice GetTotalOnSalePrice()
    {
        return new TotalOnSalePrice(Lines.Sum(l => l.PriceInfo.OnSalePrice.Value));
    }

    public Result UpdateBillingInformation(SupplierBillingInformation supplier, CustomerBillingInformation customer)
    {
        Supplier = supplier;
        Customer = customer;
        return Result.Success();
    }

    public Result SetDueOn(InvoiceDueDate dueOn, DateTimeOffset? currentDateTime = null)
    {
        if (dueOn.Value < (currentDateTime ?? DateTimeOffset.UtcNow))
            return Result.Failure(ErrorKind.BadRequest, "invoice.due.date.requires.incoming.date");

        DueOn = dueOn;
        return Result.Success();
    }
}