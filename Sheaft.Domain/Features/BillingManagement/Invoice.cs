﻿using Sheaft.Domain.OrderManagement;

namespace Sheaft.Domain.InvoiceManagement;

public class Invoice : AggregateRoot
{
    private Invoice()
    {
    }

    private Invoice(InvoiceKind kind, SupplierBillingInformation supplier,
        CustomerBillingInformation customer, string? comment = null)
    {
        Identifier = InvoiceId.New();
        Kind = kind;
        Status = InvoiceStatus.Draft;
        Supplier = supplier;
        Customer = customer;
        Comment = comment;
    }

    private Invoice(InvoiceKind kind, SupplierBillingInformation supplier,
        CustomerBillingInformation customer, IEnumerable<InvoiceLine>? lines, string? comment = null)
        : this(kind, supplier, customer, comment)
    {
        var result = SetLines(lines);
        if (result.IsFailure)
            throw new InvalidOperationException(result.Error.Code);
    }

    public static Invoice CreateInvoiceDraftForOrder(SupplierBillingInformation supplier,
        CustomerBillingInformation customer, IEnumerable<InvoiceLine> lines)
    {
        return new Invoice(InvoiceKind.Invoice, supplier, customer, lines);
    }

    public static Invoice CreateCreditNoteDraft(SupplierBillingInformation supplier, 
        CustomerBillingInformation customer, IEnumerable<InvoiceLine>? lines = null, Invoice? invoice = null)
    {
        var creditNote = new Invoice(InvoiceKind.CreditNote, supplier, customer, lines);
        if (invoice == null) 
            return creditNote;
        
        var result = invoice.AttachCreditNote(creditNote);
        if (result.IsFailure)
            throw new InvalidOperationException(result.Error.Code);

        return creditNote;
    }

    private Result AttachCreditNote(Invoice creditNote)
    {
        var notes = CreditNotes.ToList();
        if (notes.Any(n => n.InvoiceIdentifier == creditNote.Identifier))
            return Result.Failure(ErrorKind.BadRequest, "invoice.already.contains.creditnote");
        
        notes.Add(new InvoiceCreditNote(creditNote.Identifier));
        return Result.Success();
    }

    public static Result<Invoice> CancelInvoice(Invoice invoice, InvoiceReference creditNoteReference,
        string cancellationReason, DateTimeOffset? currentDateTime = null)
    {
        var lines = invoice.Lines
            .Select(v => InvoiceLine.CreateLineForDeliveryOrder(v.Identifier, v.Name, v.Quantity, 
                v.PriceInfo.UnitPrice, v.Vat,
                new InvoiceDelivery(v.Delivery.Reference, v.Delivery.DeliveredOn),
                new DeliveryOrder(v.Order.Reference, v.Order.PublishedOn)))
            .ToList();

        var creditNote = new Invoice(InvoiceKind.InvoiceCancellation, 
            SupplierBillingInformation.Copy(invoice.Supplier),
            CustomerBillingInformation.Copy(invoice.Customer),
            lines,
            comment: $"Avoir sur la facture n°{invoice.Reference.Value} du {invoice.PublishedOn.Value:d}");

        creditNote.Publish(creditNoteReference, currentDateTime);

        var result = invoice.Cancel(creditNote.Identifier, cancellationReason, currentDateTime);
        return result.IsFailure
            ? Result.Failure<Invoice>(result)
            : Result.Success(creditNote);
    }

    public InvoiceId Identifier { get; }
    public InvoiceReference? Reference { get; private set; }
    public InvoiceDueDate? DueDate { get; private set; }
    public InvoiceStatus Status { get; private set; }
    public InvoiceKind Kind { get; private set; }
    public DateTimeOffset? PublishedOn { get; private set; }
    public DateTimeOffset? CancelledOn { get; private set; }
    public DateTimeOffset? SentOn { get; private set; }
    public string? Comment { get; }
    public string? CancellationReason { get; private set; }
    public TotalWholeSalePrice TotalWholeSalePrice { get; private set; }
    public TotalVatPrice TotalVatPrice { get; private set; }
    public TotalOnSalePrice TotalOnSalePrice { get; private set; }
    public CustomerBillingInformation Customer { get; private set; }
    public SupplierBillingInformation Supplier { get; private set; }
    public IEnumerable<InvoiceLine> Lines { get; private set; } = new List<InvoiceLine>();
    public IEnumerable<InvoiceCreditNote> CreditNotes { get; private set; } = new List<InvoiceCreditNote>();
    public IEnumerable<InvoicePayment> Payments { get; private set; } = new List<InvoicePayment>();

    internal Result Publish(InvoiceReference reference, DateTimeOffset? currentDateTime = null)
    {
        if (Status != InvoiceStatus.Draft)
            return Result.Failure(ErrorKind.BadRequest, "invoice.publish.requires.draft");

        if (!Lines.Any())
            return Result.Failure(ErrorKind.BadRequest, "invoice.publish.requires.lines");

        Reference = reference;
        Status = InvoiceStatus.Published;
        PublishedOn = currentDateTime ?? DateTimeOffset.UtcNow;
        DueDate = new InvoiceDueDate((currentDateTime ?? DateTimeOffset.UtcNow).AddDays(30));
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

    public Result UpdateLines(IEnumerable<InvoiceLine>? lines)
    {
        if (Status != InvoiceStatus.Draft)
            return Result.Failure(ErrorKind.BadRequest, "invoice.update.lines.requires.draft");

        if (Kind != InvoiceKind.CreditNote)
            return Result.Failure(ErrorKind.BadRequest, "invoice.update.lines.requires.credit.note");

        SetLines(lines);
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

    public Result UpdatePaymentInformation(InvoiceDueDate dueOn, DateTimeOffset? currentDateTime = null)
    {
        if (dueOn.Value < (currentDateTime ?? DateTimeOffset.UtcNow))
            return Result.Failure(ErrorKind.BadRequest, "invoice.due.date.requires.incoming.date");

        DueDate = dueOn;
        return Result.Success();
    }
}