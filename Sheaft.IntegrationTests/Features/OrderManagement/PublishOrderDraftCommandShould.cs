using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;
using Sheaft.Application.OrderManagement;
using Sheaft.Domain;
using Sheaft.Domain.OrderManagement;
using Sheaft.Infrastructure.AgreementManagement;
using Sheaft.Infrastructure.OrderManagement;
using Sheaft.Infrastructure.Persistence;
using Sheaft.IntegrationTests.Helpers;

namespace Sheaft.IntegrationTests.OrderManagement;

#pragma warning disable CS8767
#pragma warning disable CS8618

public class PublishOrderDraftCommandShould
{
    [Test]
    public async Task Switch_Order_Status_To_Pending()
    {
        var (context, handler) = InitHandler();

        var order = InitDraft(context, true);

        var result =
            await handler.Handle(
                new PublishOrderDraftCommand(order.Identifier,
                    new OrderDeliveryDate(new DateTimeOffset(new DateTime(2022,4,1, 0, 0, 0, DateTimeKind.Utc)), new DateTimeOffset(new DateTime(2022,4,1, 0, 0, 0, DateTimeKind.Utc)))),
                CancellationToken.None);
        Assert.IsTrue(result.IsSuccess);

        Assert.IsNotNull(order);
        Assert.AreEqual(OrderStatus.Pending, order.Status);
    }

    [Test]
    public async Task Fail_If_No_Products()
    {
        var (context, handler) = InitHandler();

        var order = InitDraft(context, false);

        var result =
            await handler.Handle(
                new PublishOrderDraftCommand(order.Identifier,
                    new OrderDeliveryDate(new DateTimeOffset(new DateTime(2022,4,1, 0, 0, 0, DateTimeKind.Utc)), new DateTimeOffset(new DateTime(2022,4,1, 0, 0, 0, DateTimeKind.Utc)))),
                CancellationToken.None);
        Assert.IsTrue(result.IsFailure);

        Assert.IsNotNull(order);
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
                    new OrderDeliveryDate(new DateTimeOffset(new DateTime(2022,4,2, 0, 0, 0, DateTimeKind.Utc)), new DateTimeOffset(new DateTime(2022,4,1, 0, 0, 0, DateTimeKind.Utc)))),
                CancellationToken.None);
        
        Assert.IsTrue(result.IsFailure);
        Assert.IsNotNull(order);
        Assert.AreEqual(ErrorKind.BadRequest, result.Error.Kind);
        Assert.AreEqual("validate.order.deliveryday.not.in.agreement", result.Error.Code);
    }

    private (AppDbContext, PublishOrderDraftHandler) InitHandler()
    {
        var (context, uow, logger) = DependencyHelpers.InitDependencies<PublishOrderDraftHandler>();

        var supplier = AccountId.New();
        var customer = AccountId.New();

        var agreements = new Dictionary<AccountId, DeliveryDay> {{customer, new DeliveryDay(DayOfWeek.Friday)}};
        var supplierProducts = new Dictionary<string, int> {{"001", 2000}, {"002", 3500}};

        DataHelpers.InitContext(context,
            new List<AccountId> {customer},
            new Dictionary<AccountId, Dictionary<AccountId, DeliveryDay>> {{supplier, agreements}},
            new Dictionary<AccountId, Dictionary<string, int>> {{supplier, supplierProducts}});

        var handler =
            new PublishOrderDraftHandler(uow, 
                new PublishOrders(
                    new GenerateOrderCode(), 
                    new TransformProductsToOrderLines(context), 
                    new ValidateOrderDeliveryDate(new AgreementRepository(context))));

        return (context, handler);
    }

    private static Order InitDraft(AppDbContext context, bool addProducts)
    {
        var supplier = context.Suppliers.First();
        var customer = context.Customers.First();

        var order = Order.CreateDraft(supplier.Identifier, customer.Identifier, customer.DeliveryAddress,
            new BillingAddress("", null, "", ""));

        if (addProducts)
            order.UpdateDraftLines(new List<OrderLine>
            {
                new OrderLine(new ProductId("test 1"), new ProductCode("test 1"), new ProductName("test 1"),
                    new Quantity(1),
                    new Price(2000), new VatRate(2000)),
                new OrderLine(new ProductId("test 2"), new ProductCode("test 2"), new ProductName("test 2"),
                    new Quantity(1),
                    new Price(2000), new VatRate(2000))
            });

        context.Add(order);
        context.SaveChanges();
        return order;
    }
}
#pragma warning restore CS8767
#pragma warning restore CS8618