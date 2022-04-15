using Sheaft.Domain.InvoiceManagement;
using Sheaft.Domain.OrderManagement;

namespace Sheaft.Domain.BillingManagement;

public interface ICreateInvoices
{
    Task<Result<Invoice>> CreateDraftForDelivery(DeliveryId deliveryIdentifier, CancellationToken token);
}

public class CreateInvoices : ICreateInvoices
{
    private readonly IInvoiceRepository _invoiceRepository;
    private readonly IDeliveryRepository _deliveryRepository;
    private readonly IOrderRepository _orderRepository;
    private readonly IRetrieveBillingInformation _retrieveBillingInformation;

    public CreateInvoices(
        IInvoiceRepository invoiceRepository,
        IDeliveryRepository deliveryRepository,
        IOrderRepository orderRepository,
        IRetrieveBillingInformation retrieveBillingInformation)
    {
        _invoiceRepository = invoiceRepository;
        _deliveryRepository = deliveryRepository;
        _orderRepository = orderRepository;
        _retrieveBillingInformation = retrieveBillingInformation;
    }

    public async Task<Result<Invoice>> CreateDraftForDelivery(DeliveryId deliveryIdentifier, CancellationToken token)
    {
        var deliveryResult = await _deliveryRepository.Get(deliveryIdentifier, token);
        if (deliveryResult.IsFailure)
            return Result.Failure<Invoice>(deliveryResult);

        var delivery = deliveryResult.Value;
        
        var supplierBillingResult = await _retrieveBillingInformation.GetSupplierBilling(delivery.SupplierIdentifier, token);
        if (supplierBillingResult.IsFailure)
            return Result.Failure<Invoice>(supplierBillingResult);
        
        var customerBillingResult = await _retrieveBillingInformation.GetCustomerBilling(delivery.CustomerIdentifier, token);
        if (customerBillingResult.IsFailure)
            return Result.Failure<Invoice>(customerBillingResult);
        
        var ordersResult = await _orderRepository.Get(delivery.Identifier, token);
        if (ordersResult.IsFailure)
            return Result.Failure<Invoice>(ordersResult);
        
        var orderToInvoices = GetOrderToInvoices(delivery);
        var invoice = Invoice.CreateInvoiceDraftForOrder(supplierBillingResult.Value, customerBillingResult.Value, orderToInvoices);

        foreach (var order in ordersResult.Value)
        {
            order.AttachInvoice(invoice.Identifier);
            _orderRepository.Update(order);
        }

        _invoiceRepository.Add(invoice);
        return Result.Success(invoice);
    }

    private static List<OrderToInvoice> GetOrderToInvoices(Delivery delivery)
    {
        var orderToInvoices = new List<OrderToInvoice>();
        foreach (var order in delivery.Orders)
        {
            var lines = new List<LockedInvoiceLine>();
            foreach (var deliveryLine in delivery.Lines)
            {
                lines.Add(new LockedInvoiceLine(deliveryLine.Identifier, deliveryLine.Name, deliveryLine.Quantity,
                    deliveryLine.PriceInfo.UnitPrice, deliveryLine.Vat));
            }

            foreach (var deliveryAdjustment in delivery.Adjustments)
            {
                lines.Add(new LockedInvoiceLine(deliveryAdjustment.Identifier, deliveryAdjustment.Name,
                    deliveryAdjustment.Quantity,
                    deliveryAdjustment.PriceInfo.UnitPrice, deliveryAdjustment.Vat));
            }

            orderToInvoices.Add(new OrderToInvoice(order.Reference, order.PublishedOn, lines));
        }

        return orderToInvoices;
    }
}