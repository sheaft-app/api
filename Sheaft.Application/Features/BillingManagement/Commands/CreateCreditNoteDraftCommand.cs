using Sheaft.Domain;
using Sheaft.Domain.BillingManagement;

namespace Sheaft.Application.BillingManagement;

public record CreateCreditNoteDraftCommand(InvoiceId InvoiceIdentifier) : Command<Result<string>>;

public class CreateCreditNoteDraftHandler : ICommandHandler<CreateCreditNoteDraftCommand, Result<string>>
{
    private readonly IUnitOfWork _uow;
    private readonly IRetrieveBillingInformation _retrieveBillingInformation;

    public CreateCreditNoteDraftHandler(
        IUnitOfWork uow,
        IRetrieveBillingInformation retrieveBillingInformation)
    {
        _uow = uow;
        _retrieveBillingInformation = retrieveBillingInformation;
    }

    public async Task<Result<string>> Handle(CreateCreditNoteDraftCommand request, CancellationToken token)
    {
        var invoiceResult = await _uow.Invoices.Get(request.InvoiceIdentifier, token);
        if (invoiceResult.IsFailure)
            return Result.Failure<string>(invoiceResult);

        var invoice = invoiceResult.Value;

        var supplierBillingResult =
            await _retrieveBillingInformation.GetSupplierBilling(invoice.Supplier.Identifier, token);
        if (supplierBillingResult.IsFailure)
            return Result.Failure<string>(supplierBillingResult);

        var customerBillingResult =
            await _retrieveBillingInformation.GetCustomerBilling(invoice.Customer.Identifier, token);
        if (customerBillingResult.IsFailure)
            return Result.Failure<string>(customerBillingResult);

        var creditNote = invoice.CreateCreditNoteDraft(supplierBillingResult.Value, customerBillingResult.Value);
        
        _uow.Invoices.Add(creditNote);
        _uow.Invoices.Update(invoice);

        var result = await _uow.Save(token);
        return result.IsSuccess
            ? Result.Success(creditNote.Id.Value)
            : Result.Failure<string>(result);
    }
}