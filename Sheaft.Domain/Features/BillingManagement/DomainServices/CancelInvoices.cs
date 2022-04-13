using Sheaft.Domain.InvoiceManagement;

namespace Sheaft.Domain.BillingManagement;

public interface ICancelInvoices
{
    Task<Result<string>> Cancel(InvoiceId invoiceIdentifier, string cancellationReason, DateTimeOffset currentDateTime, CancellationToken token);
}

public class CancelInvoices : ICancelInvoices
{
    private readonly IInvoiceRepository _invoiceRepository;
    private readonly IGenerateCreditNoteCode _generateCreditNoteCode;

    public CancelInvoices(
        IInvoiceRepository invoiceRepository,
        IGenerateCreditNoteCode generateCreditNoteCode)
    {
        _invoiceRepository = invoiceRepository;
        _generateCreditNoteCode = generateCreditNoteCode;
    }

    public async Task<Result<string>> Cancel(InvoiceId invoiceIdentifier, string cancellationReason, DateTimeOffset currentDateTime, CancellationToken token)
    {
        var invoiceResult = await _invoiceRepository.Get(invoiceIdentifier, token);
        if (invoiceResult.IsFailure)
            return Result.Failure<string>(invoiceResult);

        var invoice = invoiceResult.Value;
        
        var codeResult = await _generateCreditNoteCode.GenerateNextCode(invoice.SupplierIdentifier, token);
        if (codeResult.IsFailure)
            return Result.Failure<string>(codeResult);

        var creditNoteResult = Invoice.CancelInvoice(invoice, codeResult.Value, cancellationReason, currentDateTime);
        if (creditNoteResult.IsFailure)
            return Result.Failure<string>(creditNoteResult);
        
        _invoiceRepository.Update(invoice);
        _invoiceRepository.Add(creditNoteResult.Value);
        
        return Result.Success(creditNoteResult.Value.Identifier.Value);
    }
}