using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;
using Sheaft.Application.BillingManagement;
using Sheaft.Domain;
using Sheaft.Domain.BillingManagement;
using Sheaft.Domain.OrderManagement;
using Sheaft.Infrastructure.AccountManagement;
using Sheaft.Infrastructure.BillingManagement;
using Sheaft.Infrastructure.OrderManagement;
using Sheaft.Infrastructure.Persistence;
using Sheaft.UnitTests.Helpers;

namespace Sheaft.UnitTests.BillingManagement;

#pragma warning disable CS8767
#pragma warning disable CS8618

public class CreateInvoiceForDeliveryCommandShould
{
    [Test]
    public async Task Insert_Invoice_With_Status_Published()
    {
        var (deliveryId, context, handler) = InitHandler();
        var command = new CreateInvoiceForDeliveryCommand(deliveryId);

        var result = await handler.Handle(command, CancellationToken.None);

        Assert.IsTrue(result.IsSuccess);
        var invoice = context.Invoices.Single(s => s.Id == new InvoiceId(result.Value));
        Assert.IsNotNull(invoice);
        Assert.AreEqual(InvoiceStatus.Published, invoice.Status);
    }

    [Test]
    public async Task Assign_InvoiceIdentifier_To_Delivery_Orders()
    {
        var (deliveryId, context, handler) = InitHandler();
        var command = new CreateInvoiceForDeliveryCommand(deliveryId);

        var result = await handler.Handle(command, CancellationToken.None);

        Assert.IsTrue(result.IsSuccess);
        var order = context.Orders.Single(s => s.InvoiceId == new InvoiceId(result.Value));
        Assert.IsNotNull(order);
    }

    [Test]
    public async Task Fail_If_Order_Already_Billed()
    {
        var (deliveryId, context, handler) = InitHandler(true);
        var command = new CreateInvoiceForDeliveryCommand(deliveryId);

        var result = await handler.Handle(command, CancellationToken.None);

        Assert.IsTrue(result.IsFailure);
        Assert.AreEqual(ErrorKind.BadRequest, result.Error.Kind);
        Assert.AreEqual("invoice.order.already.billed", result.Error.Code);
    }

    private (DeliveryId, AppDbContext, CreateInvoiceForDeliveryHandler) InitHandler(bool createInvoice = false)
    {
        var (context, uow, _) = DependencyHelpers.InitDependencies<CreateInvoiceForDeliveryHandler>();

        var handler = new CreateInvoiceForDeliveryHandler(
            uow,
            new CreateInvoices(
                new InvoiceRepository(context), 
                new DeliveryRepository(context),
                new OrderRepository(context), 
                new GenerateInvoiceCode(context),
                new RetrieveBillingInformation(context)));

        var supplierAccount  = DataHelpers.GetDefaultAccount(new PasswordHasher("super_password"), $"{Guid.NewGuid():N}@test.com");
        context.Add(supplierAccount);

        var customerAccount  = DataHelpers.GetDefaultAccount(new PasswordHasher("super_password"), $"{Guid.NewGuid():N}@test.com");
        context.Add(customerAccount);

        var agreements = new Dictionary<AccountId, DeliveryDay> {{customerAccount.Id, new DeliveryDay(DayOfWeek.Friday)}};
        var supplierProducts = new Dictionary<string, int> {{"001", 2000}, {"002", 3500}};

        DataHelpers.InitContext(context,
            new List<AccountId> {customerAccount.Id},
            new Dictionary<AccountId, Dictionary<AccountId, DeliveryDay>> {{supplierAccount.Id, agreements}},
            new Dictionary<AccountId, Dictionary<string, int>> {{supplierAccount.Id, supplierProducts}});

        var supplier = context.Suppliers.First();
        var customer = context.Customers.First();

        var order = DataHelpers.CreateOrderWithLines(supplier, customer, false, context.Products.ToList());
        context.Add(order);

        order.Publish(new OrderReference(0), order.Lines);

        var delivery = new Delivery(new DeliveryDate(DateTimeOffset.UtcNow.AddDays(2)),
            new DeliveryAddress("test", new EmailAddress("ese@ese.com"), "street", "", "70000", "Test"),
            order.SupplierId, order.CustomerId, new List<Order> {order});

        var product = context.Products.First();
        delivery.UpdateLines(new List<DeliveryLine>
        {
            DeliveryLine.CreateProductLine(product.Id, product.Reference, product.Name, new Quantity(2), new ProductUnitPrice(2000),
                new VatRate(0), new DeliveryOrder(new OrderReference(0), DateTimeOffset.UtcNow), null)
        });

        delivery.Schedule(new DeliveryReference(0), new DeliveryDate(DateTimeOffset.UtcNow.AddDays(2)));
        delivery.Deliver(new List<DeliveryLine>());

        if (createInvoice)
        {
            var invoice = Invoice.CreateInvoiceForOrder(
                DataHelpers.GetDefaultSupplierBilling(supplier.Id),
                DataHelpers.GetDefaultCustomerBilling(customer.Id),
                order.Lines.Select(l => InvoiceLine.CreateLineForDeliveryOrder(l.Identifier, l.Name, l.Quantity,
                    l.PriceInfo.UnitPrice, l.Vat, new InvoiceDelivery(delivery.Reference, delivery.DeliveredOn.Value),
                    new DeliveryOrder(order.Reference, order.PublishedOn.Value))).ToList(),
                new InvoiceReference(0));
            
            context.Invoices.Add(invoice);
            
            order.AttachInvoice(invoice.Id);
        }

        context.Add(delivery);

        context.SaveChanges();

        return (delivery.Id, context, handler);
    }
}

#pragma warning restore CS8767
#pragma warning restore CS8618