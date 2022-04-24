using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;
using Sheaft.Application.OrderManagement;
using Sheaft.Domain;
using Sheaft.Domain.OrderManagement;
using Sheaft.Infrastructure.OrderManagement;
using Sheaft.Infrastructure.Persistence;
using Sheaft.IntegrationTests.Helpers;

namespace Sheaft.IntegrationTests.OrderManagement;

#pragma warning disable CS8767
#pragma warning disable CS8618

public class PublishOrderDraftCommandShould
{
    [Test]
    public async Task Set_Order_Status_To_Pending()
    {
        var (context, handler) = InitHandler();

        var order = InitDraft(context, true);

        var result =
            await handler.Handle(
                new PublishOrderDraftCommand(order.Identifier,
                    new DeliveryDate(new DateTimeOffset(new DateTime(2022, 4, 1, 0, 0, 0, DateTimeKind.Utc)),
                        new DateTimeOffset(new DateTime(2022, 4, 1, 0, 0, 0, DateTimeKind.Utc)))),
                CancellationToken.None);

        Assert.IsTrue(result.IsSuccess);
        Assert.AreEqual(OrderStatus.Pending, order.Status);
    }

    [Test]
    public async Task Create_Pending_Delivery()
    {
        var (context, handler) = InitHandler();
        var order = InitDraft(context, true);

        var result =
            await handler.Handle(
                new PublishOrderDraftCommand(order.Identifier,
                    new DeliveryDate(new DateTimeOffset(new DateTime(2022, 4, 1, 0, 0, 0, DateTimeKind.Utc)),
                        new DateTimeOffset(new DateTime(2022, 4, 1, 0, 0, 0, DateTimeKind.Utc)))),
                CancellationToken.None);

        Assert.IsTrue(result.IsSuccess);

        var delivery = context.Deliveries.Single(d => d.Identifier == order.DeliveryIdentifier);

        Assert.IsNotNull(delivery);
        Assert.AreEqual(OrderStatus.Pending, order.Status);
        Assert.AreEqual(DeliveryStatus.Pending, delivery.Status);
    }

    [Test]
    public async Task Update_Order_With_Products_From_Order_Supplier_Only()
    {
        var (context, handler) = InitHandler();
        var order = InitDraft(context, true);

        var lines = context.Products
            .Select(p => new ProductQuantityDto(p.Identifier.Value, 2))
            .ToList();

        var result =
            await handler.Handle(
                new PublishOrderDraftCommand(order.Identifier,
                    new DeliveryDate(new DateTimeOffset(new DateTime(2022, 4, 1, 0, 0, 0, DateTimeKind.Utc)),
                        new DateTimeOffset(new DateTime(2022, 4, 1, 0, 0, 0, DateTimeKind.Utc))), lines),
                CancellationToken.None);

        Assert.IsTrue(result.IsSuccess);
        Assert.AreEqual(2, order.Lines.Count());
    }

    [Test]
    public async Task Fail_If_Order_Is_Not_A_Draft()
    {
        var (context, handler) = InitHandler();

        var order = InitDraft(context, true);
        order.Publish(new OrderReference(0), order.Lines);
        context.SaveChanges();

        var result =
            await handler.Handle(
                new PublishOrderDraftCommand(order.Identifier,
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
        var (context, handler) = InitHandler();

        var order = InitDraft(context, false);

        var result =
            await handler.Handle(
                new PublishOrderDraftCommand(order.Identifier,
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
        var (context, handler) = InitHandler();

        var order = InitDraft(context, true);

        var result =
            await handler.Handle(
                new PublishOrderDraftCommand(order.Identifier,
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
        var (context, handler) = InitHandler();

        var order = InitDraft(context, true);

        var agreement = context.Agreements.Single();
        agreement.Revoke("reason");
        context.SaveChanges();

        var result =
            await handler.Handle(
                new PublishOrderDraftCommand(order.Identifier,
                    new DeliveryDate(new DateTimeOffset(new DateTime(2022, 4, 1, 0, 0, 0, DateTimeKind.Utc)),
                        new DateTimeOffset(new DateTime(2022, 4, 1, 0, 0, 0, DateTimeKind.Utc)))),
                CancellationToken.None);

        Assert.IsTrue(result.IsFailure);
        Assert.AreEqual(ErrorKind.BadRequest, result.Error.Kind);
        Assert.AreEqual("order.requires.agreement", result.Error.Code);
    }

    private (AppDbContext, PublishOrderDraftHandler) InitHandler()
    {
        var (context, uow, logger) = DependencyHelpers.InitDependencies<PublishOrderDraftHandler>();

        var supplier = AccountId.New();
        var supplier1 = AccountId.New();
        var customer = AccountId.New();

        var agreements = new Dictionary<AccountId, DeliveryDay> {{customer, new DeliveryDay(DayOfWeek.Friday)}};
        var supplierProducts = new Dictionary<string, int> {{"001", 2000}, {"002", 3500}};
        var supplier1Products = new Dictionary<string, int> {{"T1", 1000}, {"T2", 5000}};

        DataHelpers.InitContext(context,
            new List<AccountId> {customer},
            new Dictionary<AccountId, Dictionary<AccountId, DeliveryDay>>
                {{supplier, agreements}, {supplier1, new Dictionary<AccountId, DeliveryDay>()}},
            new Dictionary<AccountId, Dictionary<string, int>>
                {{supplier, supplierProducts}, {supplier1, supplier1Products}});

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

        return (context, handler);
    }

    private static Order InitDraft(AppDbContext context, bool addProducts)
    {
        var supplier = context.Suppliers.First();
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