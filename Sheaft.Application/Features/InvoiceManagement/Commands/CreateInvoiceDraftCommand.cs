using Sheaft.Domain;
using Sheaft.Domain.InvoiceManagement;

namespace Sheaft.Application.InvoiceManagement;

public record CreateInvoiceDraftCommand
    (SupplierId SupplierIdentifier, CustomerId CustomerIdentifier) : Command<Result<string>>;

public class CreateInvoiceDraftHandler : ICommandHandler<CreateInvoiceDraftCommand, Result<string>>
{
    private readonly IUnitOfWork _uow;

    public CreateInvoiceDraftHandler(IUnitOfWork uow)
    {
        _uow = uow;
    }

    public async Task<Result<string>> Handle(CreateInvoiceDraftCommand request, CancellationToken token)
    {
        var customerResult = await _uow.Customers.Get(request.CustomerIdentifier, token);
        if (customerResult.IsFailure)
            return Result.Failure<string>(customerResult);

        var customer = customerResult.Value;
        var invoice = Invoice.CreateDraft(request.SupplierIdentifier, request.CustomerIdentifier,
            new BillingInformation(customer.TradeName, customer.Legal.Siret,
                new BillingAddress(customer.Legal.Address.Street, customer.Legal.Address.Complement,
                    customer.Legal.Address.Postcode, customer.Legal.Address.City)));

        _uow.Invoices.Add(invoice);
        var result = await _uow.Save(token);

        return result.IsSuccess
            ? Result.Success(invoice.Identifier.Value)
            : Result.Failure<string>(result);
    }
}