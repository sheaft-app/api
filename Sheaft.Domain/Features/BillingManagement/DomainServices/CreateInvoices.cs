using Sheaft.Domain.InvoiceManagement;
using Sheaft.Domain.OrderManagement;

namespace Sheaft.Domain.BillingManagement;

public interface ICreateInvoices
{
    Task<Result<Invoice>> CreateForDelivery(DeliveryId deliveryIdentifier, CancellationToken token);
}

public class CreateInvoices : ICreateInvoices
{
    private readonly IInvoiceRepository _invoiceRepository;
    private readonly IDeliveryRepository _deliveryRepository;
    private readonly IOrderRepository _orderRepository;
    private readonly IGenerateInvoiceCode _generateInvoiceCode;
    private readonly IRetrieveBillingInformation _retrieveBillingInformation;

    public CreateInvoices(
        IInvoiceRepository invoiceRepository,
        IDeliveryRepository deliveryRepository,
        IOrderRepository orderRepository,
        IGenerateInvoiceCode generateInvoiceCode,
        IRetrieveBillingInformation retrieveBillingInformation)
    {
        _invoiceRepository = invoiceRepository;
        _deliveryRepository = deliveryRepository;
        _orderRepository = orderRepository;
        _generateInvoiceCode = generateInvoiceCode;
        _retrieveBillingInformation = retrieveBillingInformation;
    }

    public async Task<Result<Invoice>> CreateForDelivery(DeliveryId deliveryIdentifier, CancellationToken token)
    {
        var deliveryResult = await _deliveryRepository.Get(deliveryIdentifier, token);
        if (deliveryResult.IsFailure)
            return Result.Failure<Invoice>(deliveryResult);

        var delivery = deliveryResult.Value;

        var supplierBillingResult =
            await _retrieveBillingInformation.GetSupplierBilling(delivery.SupplierIdentifier, token);
        if (supplierBillingResult.IsFailure)
            return Result.Failure<Invoice>(supplierBillingResult);

        var customerBillingResult =
            await _retrieveBillingInformation.GetCustomerBilling(delivery.CustomerIdentifier, token);
        if (customerBillingResult.IsFailure)
            return Result.Failure<Invoice>(customerBillingResult);

        var ordersResult = await _orderRepository.Get(delivery.Identifier, token);
        if (ordersResult.IsFailure)
            return Result.Failure<Invoice>(ordersResult);

        if (ordersResult.Value.Any(o => o.InvoiceIdentifier != null))
            return Result.Failure<Invoice>(ErrorKind.BadRequest, "invoice.order.already.billed");

        var codeResult = await _generateInvoiceCode.GenerateNextCode(supplierBillingResult.Value.Identifier, token);
        if (codeResult.IsFailure)
            return Result.Failure<Invoice>(codeResult);

        var invoiceLinesResult = GetInvoiceLines(delivery);
        if (invoiceLinesResult.IsFailure)
            return Result.Failure<Invoice>(invoiceLinesResult);
        
        var invoice = Invoice.CreateInvoiceForOrder(supplierBillingResult.Value, customerBillingResult.Value, 
                invoiceLinesResult.Value, codeResult.Value);

        foreach (var order in ordersResult.Value)
        {
            order.AttachInvoice(invoice.Identifier);
            _orderRepository.Update(order);
        }

        _invoiceRepository.Add(invoice);
        return Result.Success(invoice);
    }

    private Result<IEnumerable<InvoiceLine>> GetInvoiceLines(Delivery delivery)
    {
        var lines = new List<InvoiceLine>();

        foreach (var line in delivery.Lines)
        {
            lines.Add(InvoiceLine.CreateLineForDeliveryOrder(line.Identifier, line.Name,
                line.Quantity,
                line.PriceInfo.UnitPrice, line.Vat,
                new InvoiceDelivery(delivery.Reference, delivery.DeliveredOn.Value),
                new DeliveryOrder(line.Order.Reference, line.Order.PublishedOn)));
        }

        foreach (var line in delivery.Adjustments)
        {
            lines.Add(InvoiceLine.CreateLineForDeliveryOrder(line.Identifier, line.Name,
                line.Quantity,
                line.PriceInfo.UnitPrice, line.Vat,
                new InvoiceDelivery(delivery.Reference, delivery.DeliveredOn.Value),
                new DeliveryOrder(line.Order.Reference, line.Order.PublishedOn)));
        }

        var invoiceLines = new List<InvoiceLine>();
        var groupedLines = lines.GroupBy(l => new
            {l.Identifier, l.Delivery, l.Order, l.Vat, l.PriceInfo.UnitPrice, l.Name});

        foreach (var line in groupedLines)
            invoiceLines.Add(InvoiceLine.CreateLineForDeliveryOrder(line.Key.Identifier, line.Key.Name,
                new Quantity(line.Sum(l => l.Quantity.Value)), line.Key.UnitPrice, line.Key.Vat, line.Key.Delivery,
                line.Key.Order));

        var invoiceLinesWithInvalidQuantity = invoiceLines
            .Where(il => il.Quantity.Value < 0)
            .Select(il => new { il.Identifier, il.Order.Reference, il.Quantity.Value})
            .ToList();
        
        foreach (var invoiceLineWithInvalidQuantity in invoiceLinesWithInvalidQuantity)
        {
            var quantityToRemove = invoiceLineWithInvalidQuantity.Value;
            var validInvoiceLines = invoiceLines.Where(il =>
                il.Identifier == invoiceLineWithInvalidQuantity.Identifier &&
                il.Order.Reference != invoiceLineWithInvalidQuantity.Reference)
                .OrderByDescending(il => il.Quantity.Value)
                .ToList();

            while (quantityToRemove > 0)
            {
                var validInvoiceLine = validInvoiceLines.FirstOrDefault(c => c.Quantity.Value > 0);
                if (validInvoiceLine == null)
                    break;

                validInvoiceLine.Quantity.Update(new Quantity(validInvoiceLine.Quantity.Value - quantityToRemove));
                quantityToRemove -= validInvoiceLine.Quantity.Value;
            }

            if (quantityToRemove > 0)
                return Result.Failure<IEnumerable<InvoiceLine>>(ErrorKind.BadRequest, "invoice.lines.with.invalid.quantity");
        }

        return Result.Success(invoiceLines.Where(il => il.Quantity.Value > 0));
    }
}