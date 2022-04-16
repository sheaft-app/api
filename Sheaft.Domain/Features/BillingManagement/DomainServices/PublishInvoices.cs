namespace Sheaft.Domain.BillingManagement;

public interface IPublishInvoices
{
    Task<Result> Publish(InvoiceId invoiceIdentifier, DateTimeOffset currentDateTime, CancellationToken token);
}

public class PublishInvoices : IPublishInvoices
{
    private readonly IInvoiceRepository _invoiceRepository;
    private readonly IGenerateInvoiceCode _generateInvoiceCode;
    private readonly IRetrieveBillingInformation _retrieveBillingInformation;

    public PublishInvoices(
        IInvoiceRepository invoiceRepository,
        IGenerateInvoiceCode generateInvoiceCode,
        IRetrieveBillingInformation retrieveBillingInformation)
    {
        _invoiceRepository = invoiceRepository;
        _generateInvoiceCode = generateInvoiceCode;
        _retrieveBillingInformation = retrieveBillingInformation;
    }

    public async Task<Result> Publish(InvoiceId invoiceIdentifier, DateTimeOffset currentDateTime, CancellationToken token)
    {
        var invoiceResult = await _invoiceRepository.Get(invoiceIdentifier, token);
        if (invoiceResult.IsFailure)
            return Result.Failure(invoiceResult);

        var invoice = invoiceResult.Value;

        var customerBillingResult = await _retrieveBillingInformation.GetCustomerBilling(invoice.Customer.Identifier, token);
        if (customerBillingResult.IsFailure)
            return Result.Failure<string>(customerBillingResult);

        var supplierBillingResult = await _retrieveBillingInformation.GetSupplierBilling(invoice.Supplier.Identifier, token);
        if (supplierBillingResult.IsFailure)
            return Result.Failure<string>(supplierBillingResult);
        
        var billingResult = invoice.UpdateBillingInformation(supplierBillingResult.Value, customerBillingResult.Value);
        if (billingResult.IsFailure)
            return billingResult;
        
        var codeResult = await _generateInvoiceCode.GenerateNextCode(invoice.Supplier.Identifier, token);
        if (codeResult.IsFailure)
            return Result.Failure(codeResult);

        var publishResult = invoice.Publish(codeResult.Value, currentDateTime);
        
        if (publishResult.IsFailure)
            return Result.Failure(publishResult);

        _invoiceRepository.Update(invoice);

        return Result.Success();
    }
}