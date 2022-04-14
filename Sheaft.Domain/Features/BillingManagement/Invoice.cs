namespace Sheaft.Domain.InvoiceManagement;

public class Invoice : AggregateRoot
{
    private Invoice()
    {
    }

    private Invoice(InvoiceKind kind, InvoiceStatus status, SupplierId supplierIdentifier,
        CustomerId customerIdentifier, BillingInformation billingInformation, IEnumerable<InvoiceLine>? lines = null,
        InvoiceReference? reference = null)
    {
        Identifier = InvoiceId.New();
        Kind = kind;
        Reference = reference;
        Status = status;
        SupplierIdentifier = supplierIdentifier;
        CustomerIdentifier = customerIdentifier;
        BillingInformation = billingInformation;

        SetLines(lines);
    }

    public static Invoice CreateInvoiceDraft(SupplierId supplierIdentifier, CustomerId customerIdentifier,
        BillingInformation billingInformation)
    {
        return new Invoice(InvoiceKind.Invoice, InvoiceStatus.Draft, supplierIdentifier, customerIdentifier,
            billingInformation);
    }

    public static Result<Invoice> CancelInvoice(Invoice invoice, InvoiceReference creditNoteReference,
        string cancellationReason, DateTimeOffset? currentDateTime = null)
    {
        var lines = new List<InvoiceLine>
        {
            new InvoiceLine($"Avoir sur la facture n°{invoice.Reference?.Value} du {invoice.PublishedOn:d}",
                new Quantity(1), new UnitPrice(invoice.TotalWholeSalePrice.Value), new VatRate(0))
        };

        var creditNote = new Invoice(InvoiceKind.CreditNote, InvoiceStatus.Published, invoice.SupplierIdentifier,
            invoice.CustomerIdentifier, BillingInformation.Copy(invoice.BillingInformation), lines,
            creditNoteReference)
        {
            Vats = invoice.Vats.Select(v => new InvoiceVat(v.Vat, v.Price)).ToList()
        };

        var result = invoice.Cancel(creditNote.Identifier, cancellationReason, currentDateTime);
        if (result.IsFailure)
            return Result.Failure<Invoice>(result);

        return Result.Success(creditNote);
    }

    public InvoiceId Identifier { get; }
    public InvoiceReference? Reference { get; private set; }
    public DateTimeOffset? PublishedOn { get; private set; }
    public InvoiceStatus Status { get; private set; }
    public InvoiceKind Kind { get; private set; }
    public string? CancellationReason { get; private set; }
    public DateTimeOffset? CancelledOn { get; private set; }
    public DateTimeOffset? SentOn { get; private set; }
    public BillingInformation BillingInformation { get; private set; }
    public TotalWholeSalePrice TotalWholeSalePrice { get; private set; }
    public TotalVatPrice TotalVatPrice { get; private set; }
    public TotalOnSalePrice TotalOnSalePrice { get; private set; }
    public CustomerId CustomerIdentifier { get; private set; }
    public SupplierId SupplierIdentifier { get; private set; }
    public IEnumerable<InvoiceLine> Lines { get; private set; } = new List<InvoiceLine>();
    public IEnumerable<InvoiceVat> Vats { get; private set; } = new List<InvoiceVat>();
    public IEnumerable<InvoiceCreditNote> CreditNotes { get; private set; }
    public IEnumerable<InvoicePayment> Payments { get; private set; }

    internal Result Publish(InvoiceReference reference, IEnumerable<InvoiceLine> lines,
        DateTimeOffset? currentDateTime = null)
    {
        if (Status != InvoiceStatus.Draft)
            return Result.Failure(ErrorKind.BadRequest, "invoice.publish.requires.draft");

        if (!lines.Any())
            return Result.Failure(ErrorKind.BadRequest, "invoice.publish.requires.lines");

        SetLines(lines);

        Reference = reference;
        Status = InvoiceStatus.Published;
        PublishedOn = currentDateTime ?? DateTimeOffset.UtcNow;
        return Result.Success();
    }

    public Result UpdateDraftLines(IEnumerable<InvoiceLine> lines)
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

        var notes = CreditNotes.ToList();
        notes.Add(new InvoiceCreditNote(creditNoteIdentifier));

        CreditNotes = notes;
        CancellationReason = cancellationReason;
        Status = InvoiceStatus.Cancelled;
        CancelledOn = currentDateTime ?? DateTimeOffset.UtcNow;
        return Result.Success();
    }

    private void SetLines(IEnumerable<InvoiceLine>? lines)
    {
        Lines = lines?.ToList() ?? new List<InvoiceLine>();
        Vats = GetInvoiceVats(Lines);
        TotalWholeSalePrice = GetTotalWholeSalePrice();
        TotalVatPrice = GetTotalVatPrice();
        TotalOnSalePrice = GetTotalOnSalePrice();
    }

    private IEnumerable<InvoiceVat> GetInvoiceVats(IEnumerable<InvoiceLine> lines)
    {
        var groupedLines = lines.GroupBy(l => l.Vat);
        return groupedLines
            .Select(groupedLine =>
                new InvoiceVat(groupedLine.Key, new Price(groupedLine.Sum(gl => gl.PriceInfo.VatPrice.Value))))
            .ToList();
    }

    private TotalWholeSalePrice GetTotalWholeSalePrice()
    {
        return new TotalWholeSalePrice(Lines.Sum(l => l.PriceInfo.WholeSalePrice.Value));
    }

    private TotalOnSalePrice GetTotalOnSalePrice()
    {
        return new TotalOnSalePrice(Lines.Sum(l => l.PriceInfo.OnSalePrice.Value));
    }

    private TotalVatPrice GetTotalVatPrice()
    {
        return new TotalVatPrice(Vats.Sum(l => l.Price.Value));
    }
}