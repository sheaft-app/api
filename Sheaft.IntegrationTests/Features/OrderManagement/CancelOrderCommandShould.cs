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

public class CancelOrderCommandShould
{
    [Test]
    public async Task Switch_Order_Status_To_Refused()
    {
        var (context, handler) = InitHandler();
        var order = InitOrder(context);

        var cancelOrderCommand = new CancelOrderCommand(order.Identifier, "reason");
        var result =
            await handler.Handle(
                cancelOrderCommand,
                CancellationToken.None);
        
        Assert.IsTrue(result.IsSuccess);

        Assert.IsNotNull(order);
        Assert.AreEqual(OrderStatus.Cancelled, order.Status);
        Assert.AreEqual(cancelOrderCommand.CreatedAt, order.CompletedOn);
        Assert.AreEqual("reason", order.FailureReason);
    }
    
    [Test]
    public async Task Fail_To_Cancel_If_Not_In_Valid_Status()
    {
        var (context, handler) = InitHandler();
        var order = InitOrder(context, false);

        var result =
            await handler.Handle(
                new CancelOrderCommand(order.Identifier, "reason"),
                CancellationToken.None);
        
        Assert.IsTrue(result.IsFailure);
        Assert.AreEqual(ErrorKind.BadRequest, result.Error.Kind);
        Assert.AreEqual("order.cancel.requires.accepted.or.ready.status", result.Error.Code);
    }

    private (AppDbContext, CancelOrderHandler) InitHandler()
    {
        var (context, uow, logger) = DependencyHelpers.InitDependencies<CancelOrderHandler>();

        var supplier = AccountId.New();
        var customer = AccountId.New();

        var agreements = new Dictionary<AccountId, DeliveryDay> {{customer, new DeliveryDay(DayOfWeek.Friday)}};
        var supplierProducts = new Dictionary<string, int> {{"001", 2000}, {"002", 3500}};

        DataHelpers.InitContext(context,
            new List<AccountId> {customer},
            new Dictionary<AccountId, Dictionary<AccountId, DeliveryDay>> {{supplier, agreements}},
            new Dictionary<AccountId, Dictionary<string, int>> {{supplier, supplierProducts}});

        var handler = new CancelOrderHandler(uow);

        return (context, handler);
    }

    private static Order InitOrder(AppDbContext context, bool accept = true)
    {
        var supplier = context.Suppliers.First();
        var customer = context.Customers.First();

        var order = DataHelpers.CreateOrderWithLines(supplier, customer, false);

        if(accept)
            order.Accept();
        
        context.Add(order);
        context.SaveChanges();
        
        return order;
    }
}
#pragma warning restore CS8767
#pragma warning restore CS8618