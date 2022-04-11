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
using Sheaft.Infrastructure.BatchManagement;
using Sheaft.Infrastructure.Persistence;
using Sheaft.IntegrationTests.Helpers;

namespace Sheaft.IntegrationTests.BatchManagement;

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

        var batch = context.Batches.SingleOrDefault(b => b.Identifier == batchId);
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

        var supplierId = SupplierId.New();

        context.Add(new Batch(new BatchNumber("12"), BatchDateKind.DDC, DateOnly.FromDateTime(DateTime.UtcNow),
            supplierId));
        
        var batch = new Batch(new BatchNumber("0001"), BatchDateKind.DDC, DateOnly.FromDateTime(DateTime.UtcNow),
            supplierId);

        if (useBatch)
        {
            var order = Order.CreateDraft(supplierId, CustomerId.New());
            var delivery = new Delivery(new DeliveryDate(DateTimeOffset.UtcNow), new DeliveryAddress("", "", "", ""),
                supplierId, new List<Order> {order});

            delivery.Schedule(new DeliveryReference("test"), delivery.ScheduledAt,
                new List<DeliveryLine>
                {
                    DeliveryLine.CreateProductLine(ProductId.New(), new ProductReference("test"),
                        new ProductName("test"),
                        new Quantity(1), new ProductUnitPrice(2100), new VatRate(2000))
                }, new List<DeliveryBatch> {new DeliveryBatch(batch.Identifier, ProductId.New())}, delivery.ScheduledAt.Value);

            context.Add(order);
            context.Add(delivery);
        }

        context.Add(batch);
        context.SaveChanges();
        
        return (batch.Identifier, context, handler);
    }

    private RemoveBatchCommand GetCommand(BatchId batchIdentifier)
    {
        var command = new RemoveBatchCommand(batchIdentifier);
        return command;
    }
}

#pragma warning restore CS8767
#pragma warning restore CS8618