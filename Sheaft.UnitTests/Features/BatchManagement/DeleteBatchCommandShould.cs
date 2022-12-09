using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;
using Sheaft.Application.BatchManagement;
using Sheaft.Domain;
using Sheaft.Domain.BatchManagement;
using Sheaft.Domain.OrderManagement;
using Sheaft.Infrastructure.AccountManagement;
using Sheaft.Infrastructure.BatchManagement;
using Sheaft.Infrastructure.Persistence;
using Sheaft.UnitTests.Helpers;

namespace Sheaft.UnitTests.BatchManagement;

#pragma warning disable CS8767
#pragma warning disable CS8618

public class DeleteBatchCommandShould
{
    [Test]
    public async Task Remove_Entity()
    {
        var (batchId, context, handler) = InitHandler();
        var command = GetCommand(batchId);

        var result = await handler.Handle(command, CancellationToken.None);
        Assert.IsTrue(result.IsSuccess);

        var batch = context.Batches.SingleOrDefault(b => b.Id == batchId);
        Assert.IsNull(batch);
    }

    [Test]
    public async Task Fail_If_Batch_Already_Used()
    {
        var (batchId, context, handler) = InitHandler(true);
        var command = GetCommand(batchId);

        var result = await handler.Handle(command, CancellationToken.None);
        Assert.IsTrue(result.IsFailure);
        Assert.AreEqual(ErrorKind.BadRequest, result.Error.Kind);
        Assert.AreEqual("batch.cannot.remove.already.in.use", result.Error.Code);
    }

    private (BatchId, AppDbContext, RemoveBatchHandler) InitHandler(bool useBatch = false)
    {
        var (context, uow, _) = DependencyHelpers.InitDependencies<RemoveBatchHandler>();

        var handler = new RemoveBatchHandler(uow, new ValidateAlteringBatchCapability(context));

        var supplierAcct  = DataHelpers.GetDefaultAccount(new PasswordHasher("super_password"), $"{Guid.NewGuid():N}@test.com");
        context.Add(supplierAcct);

        var customerAcct  = DataHelpers.GetDefaultAccount(new PasswordHasher("super_password"), $"{Guid.NewGuid():N}@test.com");
        context.Add(customerAcct);

        var agreements = new Dictionary<AccountId, DeliveryDay> {{customerAcct.Id, new DeliveryDay(DayOfWeek.Friday)}};
        var supplierProducts = new Dictionary<string, int> {{"001", 2000}, {"002", 3500}};

        DataHelpers.InitContext(context,
            new List<AccountId> {customerAcct.Id},
            new Dictionary<AccountId, Dictionary<AccountId, DeliveryDay>> {{supplierAcct.Id, agreements}},
            new Dictionary<AccountId, Dictionary<string, int>> {{supplierAcct.Id, supplierProducts}});

        var supplier = context.Suppliers.First();
        var customer = context.Customers.First();
        
        context.Add(new Batch(new BatchNumber("12"), BatchDateKind.DDC, DateTime.Now, DateTime.Now, 
            supplier.Id));
        
        var batch = new Batch(new BatchNumber("0001"), BatchDateKind.DDC, DateTime.Now, DateTime.Now, 
            supplier.Id);

        if (useBatch)
        {
            var order = DataHelpers.CreateOrderWithLines(supplier, customer, false, context.Products.ToList());
            order.Publish(new OrderReference(0), order.Lines);
            
            var delivery = new Delivery(new DeliveryDate(DateTimeOffset.UtcNow),
                new DeliveryAddress("", new EmailAddress("test@est.com"), "", "", "", ""),
                supplier.Id, customer.Id, new List<Order> {order});

            delivery.UpdateLines(order.Lines.Select(o => new DeliveryLine(o.Identifier,
                o.LineKind == OrderLineKind.Product ? DeliveryLineKind.Product : DeliveryLineKind.Returnable, o.Reference,
                o.Name, o.Quantity, o.PriceInfo.UnitPrice, o.Vat,
                new DeliveryOrder(order.Reference, order.PublishedOn.Value), new List<BatchId>{batch.Id})));

            delivery.Schedule(new DeliveryReference(0), delivery.ScheduledAt, delivery.ScheduledAt.Value);

            context.Add(order);
            context.Add(delivery);
        }

        context.Add(batch);
        context.SaveChanges();
        
        return (batch.Id, context, handler);
    }

    private RemoveBatchCommand GetCommand(BatchId batchIdentifier)
    {
        var command = new RemoveBatchCommand(batchIdentifier, SupplierId.New());
        return command;
    }
}

#pragma warning restore CS8767
#pragma warning restore CS8618