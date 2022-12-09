using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;
using Sheaft.Application.OrderManagement;
using Sheaft.Domain;
using Sheaft.Domain.AccountManagement;
using Sheaft.Domain.OrderManagement;
using Sheaft.Domain.SupplierManagement;
using Sheaft.Infrastructure.AccountManagement;
using Sheaft.Infrastructure.Persistence;
using Sheaft.UnitTests.Helpers;

namespace Sheaft.UnitTests.OrderManagement;

#pragma warning disable CS8767
#pragma warning disable CS8618

public class CancelOrderCommandShould
{
    [Test]
    public async Task Set_Status_As_Cancelled_And_CompletedOn()
    {
        var (account, context, handler) = InitHandler();
        var order = InitOrder(context);

        var cancelOrderCommand = new CancelOrderCommand(order.Id);
        cancelOrderCommand.SetRequestUser(new RequestUser(true, ProfileKind.Supplier, account.Id, order.SupplierId.Value));
        var result =
            await handler.Handle(
                cancelOrderCommand,
                CancellationToken.None);
        
        Assert.IsTrue(result.IsSuccess);
        Assert.AreEqual(OrderStatus.Cancelled, order.Status);
        Assert.AreEqual(cancelOrderCommand.CreatedAt, order.CompletedOn);
    }
    
    [Test]
    public async Task Set_FailureReason()
    {
        var (account, context, handler) = InitHandler();
        var order = InitOrder(context);

        var cancelOrderCommand = new CancelOrderCommand(order.Id, "reason");
        cancelOrderCommand.SetRequestUser(new RequestUser(true, ProfileKind.Supplier, account.Id, order.SupplierId.Value));
        var result = await handler.Handle(cancelOrderCommand, CancellationToken.None);
        
        Assert.IsTrue(result.IsSuccess);
        Assert.AreEqual("reason", order.FailureReason);
    }
    
    [Test]
    public async Task Fail_If_Supplier_Cancel_Not_In_Accepted_Status()
    {
        var (account, context, handler) = InitHandler();
        var order = InitOrder(context, false);

        var cancelOrderCommand = new CancelOrderCommand(order.Id);
        cancelOrderCommand.SetRequestUser(new RequestUser(true, ProfileKind.Supplier, account.Id, order.SupplierId.Value));
        var result = await handler.Handle(cancelOrderCommand, CancellationToken.None);
        
        Assert.IsTrue(result.IsFailure);
        Assert.AreEqual(ErrorKind.BadRequest, result.Error.Kind);
        Assert.AreEqual("order.cancel.requires.accepted.or.fulfilled.status", result.Error.Code);
    }
    
    [Test]
    public async Task Fail_If_Customer_Cancel_Not_In_Pending_Status()
    {
        var (account, context, handler) = InitHandler();
        var order = InitOrder(context);

        var cancelOrderCommand = new CancelOrderCommand(order.Id);
        cancelOrderCommand.SetRequestUser(new RequestUser(true, ProfileKind.Customer, AccountId.New(), ""));
        var result = await handler.Handle(cancelOrderCommand, CancellationToken.None);
        
        Assert.IsTrue(result.IsFailure);
        Assert.AreEqual(ErrorKind.BadRequest, result.Error.Kind);
        Assert.AreEqual("order.cancel.requires.pending.status", result.Error.Code);
    }

    private (Account, AppDbContext, CancelOrderHandler) InitHandler()
    {
        var (context, uow, logger) = DependencyHelpers.InitDependencies<CancelOrderHandler>();

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

        var handler = new CancelOrderHandler(uow);

        return (supplierAccount, context, handler);
    }

    private static Order InitOrder(AppDbContext context, bool accept = true, bool fulfill = false)
    {
        var supplier = context.Suppliers.First();
        var customer = context.Customers.First();

        var order = DataHelpers.CreateOrderWithLines(supplier, customer, false, context.Products.ToList());

        if(accept)
            order.Accept();

        if(fulfill)
            order.Fulfill();
        
        context.Add(order);
        context.SaveChanges();
        
        return order;
    }
}
#pragma warning restore CS8767
#pragma warning restore CS8618