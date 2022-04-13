using Sheaft.Domain.InvoiceManagement;

namespace Sheaft.Domain.BillingManagement;

public interface IPublishInvoices
{
    Task<Result> Publish(InvoiceId invoiceIdentifier, IEnumerable<InvoiceLine>? lines, DateTimeOffset currentDateTime, CancellationToken token);
}

public class PublishOrders : IPublishInvoices
{
    private readonly IInvoiceRepository _invoiceRepository;
    private readonly IGenerateInvoiceCode _generateInvoiceCode;

    public PublishOrders(
        IInvoiceRepository invoiceRepository,
        IGenerateInvoiceCode generateInvoiceCode)
    {
        _invoiceRepository = invoiceRepository;
        _generateInvoiceCode = generateInvoiceCode;
    }

    public async Task<Result> Publish(InvoiceId invoiceIdentifier, IEnumerable<InvoiceLine>? lines, DateTimeOffset currentDateTime, CancellationToken token)
    {
        var invoiceResult = await _invoiceRepository.Get(invoiceIdentifier, token);
        if (invoiceResult.IsFailure)
            return Result.Failure(invoiceResult);

        var invoice = invoiceResult.Value;
        
        var codeResult = await _generateInvoiceCode.GenerateNextCode(invoice.SupplierIdentifier, token);
        if (codeResult.IsFailure)
            return Result.Failure(codeResult);

        var publishResult = invoice.Publish(codeResult.Value, lines != null && lines.Any() ? lines : invoice.Lines, currentDateTime);
        if (publishResult.IsFailure)
            return Result.Failure(publishResult);
        
        _invoiceRepository.Update(invoice);
        
        return Result.Success();
    }
}