using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;
using Sheaft.Application.OrderManagement;
using Sheaft.Domain;
using Sheaft.Domain.OrderManagement;
using Sheaft.Infrastructure.Persistence;
using Sheaft.IntegrationTests.Helpers;

namespace Sheaft.IntegrationTests.OrderManagement;

#pragma warning disable CS8767
#pragma warning disable CS8618

public class RefuseOrderCommandShould
{
    [Test]
    public async Task Switch_Order_Status_To_Refused()
    {
        var (context, handler) = InitHandler();
        var order = InitOrder(context);

        var refuseOrderCommand = new RefuseOrderCommand(order.Identifier, "reason");
        var result =
            await handler.Handle(
                refuseOrderCommand,
                CancellationToken.None);
        
        Assert.IsTrue(result.IsSuccess);

        Assert.IsNotNull(order);
        Assert.AreEqual(OrderStatus.Refused, order.Status);
        Assert.AreEqual("reason", order.FailureReason);
        Assert.AreEqual(refuseOrderCommand.CreatedAt, order.RefusedOn);
    }
    
    [Test]
    public async Task Fail_To_Refuse_If_Not_In_Valid_Status()
    {
        var (context, handler) = InitHandler();
        var order = InitOrder(context, true);

        var refuseOrderCommand = new RefuseOrderCommand(order.Identifier, "reason");
        var result =
            await handler.Handle(
                refuseOrderCommand,
                CancellationToken.None);
        
        Assert.IsTrue(result.IsFailure);
        Assert.AreEqual(ErrorKind.BadRequest, result.Error.Kind);
        Assert.AreEqual("order.refuse.requires.pending.status", result.Error.Code);
    }

    private (AppDbContext, RefuseOrderHandler) InitHandler()
    {
        var (context, uow, logger) = DependencyHelpers.InitDependencies<RefuseOrderHandler>();

        var supplier = AccountId.New();
        var customer = AccountId.New();

        var agreements = new Dictionary<AccountId, DeliveryDay> {{customer, new DeliveryDay(DayOfWeek.Friday)}};
        var supplierProducts = new Dictionary<string, int> {{"001", 2000}, {"002", 3500}};

        DataHelpers.InitContext(context,
            new List<AccountId> {customer},
            new Dictionary<AccountId, Dictionary<AccountId, DeliveryDay>> {{supplier, agreements}},
            new Dictionary<AccountId, Dictionary<string, int>> {{supplier, supplierProducts}});

        var handler = new RefuseOrderHandler(uow);

        return (context, handler);
    }

    private static Order InitOrder(AppDbContext context, bool accept = false)
    {
        var supplier = context.Suppliers.First();
        var customer = context.Customers.First();

        var order = DataHelpers.CreateOrderWithLines(supplier, customer, false);

        if (accept)
            order.Accept();
        
        context.Add(order);
        context.SaveChanges();
        
        return order;
    }
}
#pragma warning restore CS8767
#pragma warning restore CS8618