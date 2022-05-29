using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;
using Sheaft.Application.OrderManagement;
using Sheaft.Domain;
using Sheaft.Domain.OrderManagement;
using Sheaft.Infrastructure.AccountManagement;
using Sheaft.Infrastructure.OrderManagement;
using Sheaft.Infrastructure.Persistence;
using Sheaft.UnitTests.Helpers;

namespace Sheaft.UnitTests.OrderManagement;

#pragma warning disable CS8767
#pragma warning disable CS8618

public class PublishOrderDraftCommandShould
{
    [Test]
    public async Task Set_Order_Status_To_Pending()
    {
        var (suppAcctId, context, handler) = InitHandler();

        var order = InitDraft(context, true, suppAcctId);

        var result =
            await handler.Handle(
                new PublishOrderDraftCommand(order.Id,
                    new DeliveryDate(new DateTimeOffset(new DateTime(2022, 4, 1, 0, 0, 0, DateTimeKind.Utc)),
                        new DateTimeOffset(new DateTime(2022, 4, 1, 0, 0, 0, DateTimeKind.Utc)))),
                CancellationToken.None);

        Assert.IsTrue(result.IsSuccess);
        Assert.AreEqual(OrderStatus.Pending, order.Status);
    }

    [Test]
    public async Task Create_Pending_Delivery()
    {
        var (suppAcctId, context, handler) = InitHandler();
        var order = InitDraft(context, true,suppAcctId);

        var result =
            await handler.Handle(
                new PublishOrderDraftCommand(order.Id,
                    new DeliveryDate(new DateTimeOffset(new DateTime(2022, 4, 1, 0, 0, 0, DateTimeKind.Utc)),
                        new DateTimeOffset(new DateTime(2022, 4, 1, 0, 0, 0, DateTimeKind.Utc)))),
                CancellationToken.None);

        Assert.IsTrue(result.IsSuccess);

        var delivery = context.Deliveries.Single(d => d.Id == order.DeliveryId);

        Assert.IsNotNull(delivery);
        Assert.AreEqual(OrderStatus.Pending, order.Status);
        Assert.AreEqual(DeliveryStatus.Pending, delivery.Status);
    }

    [Test]
    public async Task Update_Order_With_Products_From_Order_Supplier_Only()
    {
        var (suppAcctId, context, handler) = InitHandler();
        var order = InitDraft(context, true,suppAcctId);

        var lines = context.Products
            .Select(p => new ProductQuantityDto(p.Id.Value, 2))
            .ToList();

        var result =
            await handler.Handle(
                new PublishOrderDraftCommand(order.Id,
                    new DeliveryDate(new DateTimeOffset(new DateTime(2022, 4, 1, 0, 0, 0, DateTimeKind.Utc)),
                        new DateTimeOffset(new DateTime(2022, 4, 1, 0, 0, 0, DateTimeKind.Utc))), lines),
                CancellationToken.None);

        Assert.IsTrue(result.IsSuccess);
        Assert.AreEqual(2, order.Lines.Count());
    }

    [Test]
    public async Task Fail_If_Order_Is_Not_A_Draft()
    {
        var (suppAcctId, context, handler) = InitHandler();

        var order = InitDraft(context, true,suppAcctId);
        order.Publish(new OrderReference(0), order.Lines);
        context.SaveChanges();

        var result =
            await handler.Handle(
                new PublishOrderDraftCommand(order.Id,
                    new DeliveryDate(new DateTimeOffset(new DateTime(2022, 4, 1, 0, 0, 0, DateTimeKind.Utc)),
                        new DateTimeOffset(new DateTime(2022, 4, 1, 0, 0, 0, DateTimeKind.Utc)))),
                CancellationToken.None);

        Assert.IsTrue(result.IsFailure);
        Assert.AreEqual(ErrorKind.BadRequest, result.Error.Kind);
        Assert.AreEqual("order.publish.requires.draft", result.Error.Code);
    }

    [Test]
    public async Task Fail_If_Order_Has_No_Products()
    {
        var (suppAcctId, context, handler) = InitHandler();

        var order = InitDraft(context, false,suppAcctId);

        var result =
            await handler.Handle(
                new PublishOrderDraftCommand(order.Id,
                    new DeliveryDate(new DateTimeOffset(new DateTime(2022, 4, 1, 0, 0, 0, DateTimeKind.Utc)),
                        new DateTimeOffset(new DateTime(2022, 4, 1, 0, 0, 0, DateTimeKind.Utc)))),
                CancellationToken.None);

        Assert.IsTrue(result.IsFailure);
        Assert.AreEqual(ErrorKind.BadRequest, result.Error.Kind);
        Assert.AreEqual("order.publish.requires.lines", result.Error.Code);
    }

    [Test]
    public async Task Fail_If_DeliveryDate_Day_Not_In_Agreement_DeliveryDay()
    {
        var (suppAcctId, context, handler) = InitHandler();

        var order = InitDraft(context, true,suppAcctId);

        var result =
            await handler.Handle(
                new PublishOrderDraftCommand(order.Id,
                    new DeliveryDate(new DateTimeOffset(new DateTime(2022, 4, 2, 0, 0, 0, DateTimeKind.Utc)),
                        new DateTimeOffset(new DateTime(2022, 4, 1, 0, 0, 0, DateTimeKind.Utc)))),
                CancellationToken.None);

        Assert.IsTrue(result.IsFailure);
        Assert.AreEqual(ErrorKind.BadRequest, result.Error.Kind);
        Assert.AreEqual("validate.order.deliveryday.not.in.agreement", result.Error.Code);
    }

    [Test]
    public async Task Fail_If_No_Active_Agreement()
    {
        var (suppAcctId, context, handler) = InitHandler();

        var order = InitDraft(context, true, suppAcctId);

        var agreement = context.Agreements.Single();
        agreement.Revoke("reason");
        context.SaveChanges();

        var result =
            await handler.Handle(
                new PublishOrderDraftCommand(order.Id,
                    new DeliveryDate(new DateTimeOffset(new DateTime(2022, 4, 1, 0, 0, 0, DateTimeKind.Utc)),
                        new DateTimeOffset(new DateTime(2022, 4, 1, 0, 0, 0, DateTimeKind.Utc)))),
                CancellationToken.None);

        Assert.IsTrue(result.IsFailure);
        Assert.AreEqual(ErrorKind.BadRequest, result.Error.Kind);
        Assert.AreEqual("order.requires.agreement", result.Error.Code);
    }

    private (AccountId, AppDbContext, PublishOrderDraftHandler) InitHandler()
    {
        var (context, uow, logger) = DependencyHelpers.InitDependencies<PublishOrderDraftHandler>();

        var supplier1Account  = DataHelpers.GetDefaultAccount(new PasswordHasher("super_password"), $"{Guid.NewGuid():N}@test.com");
        context.Add(supplier1Account);
        
        var supplier2Account  = DataHelpers.GetDefaultAccount(new PasswordHasher("super_password"), $"{Guid.NewGuid():N}@test.com");
        context.Add(supplier2Account);

        var customerAccount  = DataHelpers.GetDefaultAccount(new PasswordHasher("super_password"), $"{Guid.NewGuid():N}@test.com");
        context.Add(customerAccount);

        context.SaveChanges();
        
        var supplier1agreements = new Dictionary<AccountId, DeliveryDay> {{customerAccount.Id, new DeliveryDay(DayOfWeek.Friday)}};
        var supplier1Products = new Dictionary<string, int> {{"001", 2000}, {"002", 3500}};
        var supplier2Products = new Dictionary<string, int> {{"T1", 1000}, {"T2", 5000}};

        DataHelpers.InitContext(context,
            new List<AccountId> {customerAccount.Id},
            new Dictionary<AccountId, Dictionary<AccountId, DeliveryDay>>
                {{supplier1Account.Id, supplier1agreements}, {supplier2Account.Id, new Dictionary<AccountId, DeliveryDay>()}},
            new Dictionary<AccountId, Dictionary<string, int>>
                {{supplier1Account.Id, supplier1Products}, {supplier2Account.Id, supplier2Products}});

        var handler =
            new PublishOrderDraftHandler(uow,
                new PublishOrders(
                    new OrderRepository(context),
                    new DeliveryRepository(context),
                    new GenerateOrderCode(context),
                    new TransformProductsToOrderLines(context),
                    new ValidateOrderDeliveryDate(new RetrieveDeliveryDays(context)),
                    new RetrieveOrderCustomer(context),
                    new RetrieveAgreementForOrder(context)));

        return (supplier1Account.Id, context, handler);
    }

    private static Order InitDraft(AppDbContext context, bool addProducts, AccountId supplierId)
    {
        var supplier = context.Suppliers.First(s => s.AccountId == supplierId);
        var customer = context.Customers.First();

        var order = DataHelpers.CreateOrderWithLines(supplier, customer, true,
            addProducts ? context.Products.ToList() : null);

        context.Add(order);
        context.SaveChanges();

        return order;
    }
}
#pragma warning restore CS8767
#pragma warning restore CS8618