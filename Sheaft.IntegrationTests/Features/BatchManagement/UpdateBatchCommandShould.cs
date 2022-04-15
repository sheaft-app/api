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

public class UpdateBatchCommandShould
{
    [Test]
    public async Task Update_Info()
    {
        var time = DateTime.Now;
        var (batchId, context, handler) = InitHandler();
        var command = GetCommand(batchId, "0002", BatchDateKind.DDM, time);

        var result = await handler.Handle(command, CancellationToken.None);
        Assert.IsTrue(result.IsSuccess);

        var batch = context.Batches.Single(b => b.Identifier == batchId);
        Assert.IsNotNull(batch);
        Assert.AreEqual("0002", batch.Number.Value);
        Assert.AreEqual(BatchDateKind.DDM, batch.DateKind);
        Assert.AreEqual(DateOnly.FromDateTime(time), batch.Date);
    }

    [Test]
    public async Task Fail_If_Number_Already_Exists()
    {
        var (batchId, context, handler) = InitHandler();
        var command = GetCommand(batchId, "12", BatchDateKind.DDC, DateTime.Now);

        var result = await handler.Handle(command, CancellationToken.None);
        Assert.IsTrue(result.IsFailure);
        Assert.AreEqual(ErrorKind.Conflict, result.Error.Kind);
        Assert.AreEqual("batch.number.already.exists", result.Error.Code);
    }

    [Test]
    public async Task Fail_If_Batch_Already_Used()
    {
        var (batchId, context, handler) = InitHandler(true);
        var command = GetCommand(batchId, "0001", BatchDateKind.DDM, DateTime.Now);

        var result = await handler.Handle(command, CancellationToken.None);
        Assert.IsTrue(result.IsFailure);
        Assert.AreEqual(ErrorKind.BadRequest, result.Error.Kind);
        Assert.AreEqual("batch.cannot.update.already.in.use", result.Error.Code);
    }

    private (BatchId, AppDbContext, UpdateBatchHandler) InitHandler(bool useBatch = false)
    {
        var (context, uow, _) = DependencyHelpers.InitDependencies<UpdateBatchHandler>();

        var handler = new UpdateBatchHandler(uow, new ValidateAlteringBatchCapability(context));

        var supplierId = SupplierId.New();

        context.Add(new Batch(new BatchNumber("12"), BatchDateKind.DDC, DateOnly.FromDateTime(DateTime.UtcNow),
            supplierId));
        
        var batch = new Batch(new BatchNumber("0001"), BatchDateKind.DDC, DateOnly.FromDateTime(DateTime.UtcNow),
            supplierId);

        if (useBatch)
        {
            var order = Order.CreateDraft(supplierId, CustomerId.New());
            var delivery = new Delivery(new DeliveryDate(DateTimeOffset.UtcNow), 
                new DeliveryAddress("", new EmailAddress("test@est.com"), "", "", "", ""),
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

    private UpdateBatchCommand GetCommand(BatchId batchIdentifier, string number, BatchDateKind batchDateKind, DateTime dateTimeOffset)
    {
        var command = new UpdateBatchCommand(batchIdentifier, number, batchDateKind, DateOnly.FromDateTime(dateTimeOffset));
        return command;
    }
}

#pragma warning restore CS8767
#pragma warning restore CS8618