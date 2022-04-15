using Sheaft.Domain;
using Sheaft.Domain.InvoiceManagement;

namespace Sheaft.Application.InvoiceManagement;

public record CreateInvoiceDraftForOrdersCommand(IEnumerable<OrderId> OrderIdentifiers) : Command<Result<IEnumerable<string>>>;

public class CreateInvoiceDraftForOrdersHandler : ICommandHandler<CreateInvoiceDraftForOrdersCommand, Result<IEnumerable<string>>>
{
    private readonly IUnitOfWork _uow;
    private readonly IRetrieveBillingInformation _retrieveBillingInformation;

    public CreateInvoiceDraftForOrdersHandler(
        IUnitOfWork uow,
        IRetrieveBillingInformation retrieveBillingInformation)
    {
        _uow = uow;
        _retrieveBillingInformation = retrieveBillingInformation;
    }

    public async Task<Result<IEnumerable<string>>> Handle(CreateInvoiceDraftForOrdersCommand request, CancellationToken token)
    {
        var ordersResult = await _uow.Orders.Get(request.OrderIdentifiers, token);
        if (ordersResult.IsFailure)
            return Result.Failure<IEnumerable<string>>(ordersResult);

        var orderIdentifiers = new List<string>();
        foreach (var order in ordersResult.Value)
        {
            var supplierBillingResult = await _retrieveBillingInformation.GetSupplierBilling(order.SupplierIdentifier, token);
            if (supplierBillingResult.IsFailure)
                return Result.Failure<IEnumerable<string>>(supplierBillingResult);
        
            var customerBillingResult = await _retrieveBillingInformation.GetCustomerBilling(order.CustomerIdentifier, token);
            if (customerBillingResult.IsFailure)
                return Result.Failure<IEnumerable<string>>(customerBillingResult);

            var invoice = Invoice.CreateInvoiceDraftForOrder(supplierBillingResult.Value, customerBillingResult.Value, 
                order.Lines.Select(ol => InvoiceLine.CreateLockedLine(ol.Name, ol.Quantity, ol.PriceInfo.UnitPrice, ol.Vat, ol.Identifier)));

            _uow.Invoices.Add(invoice);
            orderIdentifiers.Add(invoice.Identifier.Value);
        }
            
        var result = await _uow.Save(token);

        return result.IsSuccess
            ? Result.Success(orderIdentifiers.AsEnumerable())
            : Result.Failure<IEnumerable<string>>(result);
    }
}