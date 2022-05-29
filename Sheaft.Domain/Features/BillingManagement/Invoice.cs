using Sheaft.Domain.OrderManagement;

namespace Sheaft.Domain.BillingManagement;

public class Invoice : AggregateRoot
{
    private Invoice()
    {
    }

    private Invoice(InvoiceKind kind, SupplierBillingInformation supplier,
        CustomerBillingInformation customer, string? comment = null)
    {
        Id = InvoiceId.New();
        Kind = kind;
        Status = InvoiceStatus.Draft;
        Supplier = supplier;
        Customer = customer;
        Comment = comment;
        CreatedOn = DateTimeOffset.UtcNow;
        UpdatedOn = DateTimeOffset.UtcNow;
    }

    private Invoice(InvoiceKind kind, SupplierBillingInformation supplier,
        CustomerBillingInformation customer, IEnumerable<InvoiceLine>? lines, string? comment = null)
        : this(kind, supplier, customer, comment)
    {
        var result = SetLines(lines);
        if (result.IsFailure)
            throw new InvalidOperationException(result.Error.Code);
    }

    public static Invoice CreateInvoiceForOrder(SupplierBillingInformation supplier,
        CustomerBillingInformation customer, IEnumerable<InvoiceLine> lines, InvoiceReference reference,
        DateTimeOffset? currentDateTime = null)
    {
        var invoice = new Invoice(InvoiceKind.Invoice, supplier, customer, lines);
        var result = invoice.Publish(reference, currentDateTime);
        if (result.IsFailure)
            throw new InvalidOperationException(result.Error.Code);

        return invoice;
    }

    public static Invoice CreateInvoice(SupplierBillingInformation supplier,
        CustomerBillingInformation customer, IEnumerable<InvoiceLine> lines)
    {
        return new Invoice(InvoiceKind.Invoice, supplier, customer, lines);
    }

    public Invoice CreateCreditNoteDraft(SupplierBillingInformation supplier,
        CustomerBillingInformation customer, IEnumerable<InvoiceLine>? lines = null)
    {
        var creditNote = new Invoice(InvoiceKind.CreditNote, supplier, customer, lines,
            $"Avoir sur la facture n°{Reference.Value} du {PublishedOn:d}");

        var result = AttachCreditNote(creditNote);
        if (result.IsFailure)
            throw new InvalidOperationException(result.Error.Code);

        return creditNote;
    }

    private Result AttachCreditNote(Invoice creditNote)
    {
        var creditNotes = CreditNotes.ToList();
        if (creditNotes.Any(n => n.Id == creditNote.Id))
            return Result.Failure(ErrorKind.BadRequest, "invoice.already.contains.creditnote");

        creditNotes.Add(creditNote);
        CreditNotes = creditNotes.ToList();
        return Result.Success();
    }

    public Result<Invoice> Cancel(CreditNoteReference creditNoteReference,
        string cancellationReason, DateTimeOffset? currentDateTime = null)
    {
        var lines = Lines
            .Select(v => InvoiceLine.CreateLineForDeliveryOrder(v.Identifier, v.Name, v.Quantity,
                v.PriceInfo.UnitPrice, v.Vat,
                new InvoiceDelivery(v.Delivery.Reference, v.Delivery.DeliveredOn),
                new DeliveryOrder(v.Order.Reference, v.Order.PublishedOn)))
            .ToList();

        var creditNote = new Invoice(InvoiceKind.InvoiceCancellation,
            SupplierBillingInformation.Copy(Supplier),
            CustomerBillingInformation.Copy(Customer),
            lines,
            $"Avoir pour l'annulation de la facture n°{Reference.Value} du {PublishedOn.Value:d}");

        var publishResult = creditNote.Publish(creditNoteReference, currentDateTime);
        if (publishResult.IsFailure)
            return Result.Failure<Invoice>(publishResult);

        if (Status != InvoiceStatus.Published && Status != InvoiceStatus.Sent)
            return Result.Failure<Invoice>(ErrorKind.BadRequest, "invoice.cancel.requires.published.or.sent");

        if (string.IsNullOrWhiteSpace(cancellationReason))
            return Result.Failure<Invoice>(ErrorKind.BadRequest, "invoice.cancel.requires.reason");

        CreditNotes = new List<Invoice> {creditNote};
        CancellationReason = cancellationReason;
        Status = InvoiceStatus.Cancelled;
        CancelledOn = currentDateTime ?? DateTimeOffset.UtcNow;
        
        return Result.Success(creditNote);
    }

    public InvoiceId Id { get; }
    public DateTimeOffset CreatedOn { get; private set; }
    public DateTimeOffset UpdatedOn { get; private set; }
    public BillingReference? Reference { get; private set; }
    public InvoiceDueDate? DueDate { get; private set; }
    public InvoiceStatus Status { get; private set; }
    public InvoiceKind Kind { get; private set; }
    public DateTimeOffset? PublishedOn { get; private set; }
    public DateTimeOffset? CancelledOn { get; private set; }
    public DateTimeOffset? SentOn { get; private set; }
    public string? Comment { get; private set; }
    public string? CancellationReason { get; private set; }
    public TotalWholeSalePrice TotalWholeSalePrice { get; private set; }
    public TotalVatPrice TotalVatPrice { get; private set; }
    public TotalOnSalePrice TotalOnSalePrice { get; private set; }
    public CustomerBillingInformation Customer { get; private set; }
    public SupplierBillingInformation Supplier { get; private set; }
    public IEnumerable<InvoiceLine> Lines { get; private set; } = new List<InvoiceLine>();
    public IEnumerable<InvoicePayment> Payments { get; private set; } = new List<InvoicePayment>();
    public IEnumerable<Invoice> CreditNotes { get; private set; } = new List<Invoice>();

    internal Result Publish(BillingReference reference, DateTimeOffset? currentDateTime = null)
    {
        if (Status != InvoiceStatus.Draft)
            return Result.Failure(ErrorKind.BadRequest, "invoice.publish.requires.draft");

        if (!Lines.Any())
            return Result.Failure(ErrorKind.BadRequest, "invoice.publish.requires.lines");
        
        if(Kind != InvoiceKind.Invoice && reference is not CreditNoteReference)
            return Result.Failure(ErrorKind.BadRequest, "creditnote.publish.requires.creditnote.reference");
        
        if(Kind == InvoiceKind.Invoice && reference is not InvoiceReference)
            return Result.Failure(ErrorKind.BadRequest, "invoice.publish.requires.invoice.reference");

        Reference = reference;
        Status = InvoiceStatus.Published;
        PublishedOn = currentDateTime ?? DateTimeOffset.UtcNow;
        DueDate = Kind == InvoiceKind.Invoice ? new InvoiceDueDate((currentDateTime ?? DateTimeOffset.UtcNow).AddDays(30)) : null;
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

    public Result MarkAsPayed(PaymentReference reference, PaymentKind kind, DateTimeOffset paymentDate)
    {
        if (Status != InvoiceStatus.Sent && Kind == InvoiceKind.Invoice)
            return Result.Failure(ErrorKind.BadRequest, "invoice.payed.requires.sent");

        if (Status != InvoiceStatus.Published && Status != InvoiceStatus.Sent && Kind == InvoiceKind.CreditNote)
            return Result.Failure(ErrorKind.BadRequest, "creditnote.payed.requires.published");

        Status = InvoiceStatus.Payed;
        Payments = new List<InvoicePayment> {new InvoicePayment(reference, kind, paymentDate)};
        return Result.Success();
    }

    private Result SetLines(IEnumerable<InvoiceLine>? lines)
    {
        Lines = lines?.ToList() ?? new List<InvoiceLine>();
        TotalWholeSalePrice = GetTotalWholeSalePrice();
        TotalVatPrice = GetTotalVatPrice();
        TotalOnSalePrice = GetTotalOnSalePrice();
        return Result.Success();
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

    public Result RemoveCreditNote(Invoice creditNote)
    {
        var existingCreditNote = CreditNotes.FirstOrDefault(c => c.Id == creditNote.Id);
        if (existingCreditNote == null)
            return Result.Failure(ErrorKind.BadRequest, "invoice.remove.creditnote.not.found");

        var creditNotes = CreditNotes.ToList();
        creditNotes.Remove(existingCreditNote);
        CreditNotes = creditNotes.ToList();
        
        return Result.Success();
    }
}