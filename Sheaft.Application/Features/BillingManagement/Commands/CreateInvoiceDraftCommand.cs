using Sheaft.Domain;
using Sheaft.Domain.InvoiceManagement;

namespace Sheaft.Application.InvoiceManagement;

public record CreateInvoiceDraftCommand
    (SupplierId SupplierIdentifier, CustomerId CustomerIdentifier) : Command<Result<string>>;

public class CreateInvoiceDraftHandler : ICommandHandler<CreateInvoiceDraftCommand, Result<string>>
{
    private readonly IUnitOfWork _uow;
    private readonly IRetrieveBillingInformation _retrieveBillingInformation;

    public CreateInvoiceDraftHandler(
        IUnitOfWork uow,
        IRetrieveBillingInformation retrieveBillingInformation)
    {
        _uow = uow;
        _retrieveBillingInformation = retrieveBillingInformation;
    }

    public async Task<Result<string>> Handle(CreateInvoiceDraftCommand request, CancellationToken token)
    {
        var supplierBillingResult = await _retrieveBillingInformation.GetSupplierBilling(request.SupplierIdentifier, token);
        if (supplierBillingResult.IsFailure)
            return Result.Failure<string>(supplierBillingResult);
        
        var customerBillingResult = await _retrieveBillingInformation.GetCustomerBilling(request.CustomerIdentifier, token);
        if (customerBillingResult.IsFailure)
            return Result.Failure<string>(customerBillingResult);

        var invoice = Invoice.CreateInvoiceDraft(supplierBillingResult.Value, customerBillingResult.Value);

        _uow.Invoices.Add(invoice);
        var result = await _uow.Save(token);

        return result.IsSuccess
            ? Result.Success(invoice.Identifier.Value)
            : Result.Failure<string>(result);
    }
}