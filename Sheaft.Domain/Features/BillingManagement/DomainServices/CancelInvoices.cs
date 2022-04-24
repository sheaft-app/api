using Sheaft.Domain.OrderManagement;

namespace Sheaft.Domain.BillingManagement;

public interface ICancelInvoices
{
    Task<Result<string>> Cancel(InvoiceId invoiceIdentifier, string cancellationReason, DateTimeOffset currentDateTime, CancellationToken token);
}

public class CancelInvoices : ICancelInvoices
{
    private readonly IInvoiceRepository _invoiceRepository;
    private readonly IOrderRepository _orderRepository;
    private readonly IGenerateCreditNoteCode _generateCreditNoteCode;

    public CancelInvoices(
        IInvoiceRepository invoiceRepository,
        IOrderRepository orderRepository,
        IGenerateCreditNoteCode generateCreditNoteCode)
    {
        _invoiceRepository = invoiceRepository;
        _orderRepository = orderRepository;
        _generateCreditNoteCode = generateCreditNoteCode;
    }

    public async Task<Result<string>> Cancel(InvoiceId invoiceIdentifier, string cancellationReason, DateTimeOffset currentDateTime, CancellationToken token)
    {
        var invoiceResult = await _invoiceRepository.Get(invoiceIdentifier, token);
        if (invoiceResult.IsFailure)
            return Result.Failure<string>(invoiceResult);

        var invoice = invoiceResult.Value;
        
        var ordersResult = await _orderRepository.Find(invoiceIdentifier, token);
        if (ordersResult.IsFailure)
            return Result.Failure<string>(ordersResult);

        foreach (var order in ordersResult.Value)
        {
            order.DetachInvoice();
            _orderRepository.Update(order);
        }
        
        var codeResult = _generateCreditNoteCode.GenerateNextCode(invoice.Supplier.Identifier);
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