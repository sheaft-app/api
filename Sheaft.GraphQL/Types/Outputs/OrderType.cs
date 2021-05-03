using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using GreenDonut;
using HotChocolate;
using HotChocolate.Resolvers;
using HotChocolate.Types;
using Microsoft.EntityFrameworkCore;
using Sheaft.Domain;
using Sheaft.GraphQL.Catalogs;
using Sheaft.GraphQL.Orders;
using Sheaft.GraphQL.PurchaseOrders;
using Sheaft.GraphQL.Tags;
using Sheaft.GraphQL.Users;
using Sheaft.Infrastructure.Persistence;

namespace Sheaft.GraphQL.Types.Outputs
{
    public class OrderType : SheaftOutputType<Order>
    {
        protected override void Configure(IObjectTypeDescriptor<Order> descriptor)
        {
            base.Configure(descriptor);

            descriptor
                .ImplementsNode()
                .IdField(c => c.Id)
                .ResolveNode((ctx, id) =>
                    ctx.DataLoader<OrdersByIdBatchDataLoader>().LoadAsync(id, ctx.RequestAborted));

            descriptor
                .Field(c => c.Status)
                .Name("status");

            descriptor
                .Field(c => c.DonationKind)
                .Name("donationKind");

            descriptor
                .Field(c => c.CreatedOn)
                .Name("createdOn");

            descriptor
                .Field(c => c.UpdatedOn)
                .Name("updatedOn");

            descriptor
                .Field(c => c.Reference)
                .Name("reference");

            descriptor
                .Field(c => c.TotalWholeSalePrice)
                .Name("totalWholeSalePrice");

            descriptor
                .Field(c => c.TotalVatPrice)
                .Name("totalVatPrice");

            descriptor
                .Field(c => c.TotalOnSalePrice)
                .Name("totalOnSalePrice");

            descriptor
                .Field(c => c.TotalReturnableWholeSalePrice)
                .Name("totalReturnableWholeSalePrice");

            descriptor
                .Field(c => c.TotalReturnableVatPrice)
                .Name("totalReturnableVatPrice");

            descriptor
                .Field(c => c.TotalReturnableOnSalePrice)
                .Name("totalReturnableOnSalePrice");

            descriptor
                .Field(c => c.TotalProductWholeSalePrice)
                .Name("totalProductWholeSalePrice");

            descriptor
                .Field(c => c.TotalProductVatPrice)
                .Name("totalProductVatPrice");

            descriptor
                .Field(c => c.TotalProductOnSalePrice)
                .Name("totalProductOnSalePrice");

            descriptor
                .Field(c => c.TotalPrice)
                .Name("totalPrice");

            descriptor
                .Field(c => c.Donation)
                .Name("donation");

            descriptor
                .Field(c => c.FeesPrice)
                .Name("feesPrice");

            descriptor
                .Field(c => c.FeesFixedAmount)
                .Name("feesFixedAmount");

            descriptor
                .Field(c => c.FeesPercent)
                .Name("feesPercent");

            descriptor
                .Field(c => c.TotalWeight)
                .Name("totalWeight");

            descriptor
                .Field(c => c.DonationFeesPrice)
                .Name("internalFeesPrice");

            descriptor
                .Field(c => c.PurchaseOrdersCount)
                .Name("purchaseOrdersCount");

            descriptor
                .Field(c => c.LinesCount)
                .Name("linesCount");

            descriptor
                .Field(c => c.ProductsCount)
                .Name("productsCount");

            descriptor
                .Field(c => c.ReturnablesCount)
                .Name("returnablesCount");

            descriptor
                .Field("totalFees")
                .ResolveWith<OrderResolvers>(c => c.GetTotalFees(default))
                .Type<NonNullType<DecimalType>>();

            descriptor.Field(c => c.User)
                .Name("user")
                .ResolveWith<OrderResolvers>(c => c.GetUser(default, default, default))
                .Type<UserType>();

            descriptor.Field(c => c.Products)
                .Name("products")
                .UseDbContext<AppDbContext>()
                .ResolveWith<OrderResolvers>(c => c.GetProducts(default, default, default, default))
                .Type<ListType<OrderProductType>>();

            descriptor.Field(c => c.Deliveries)
                .Name("deliveries")
                .UseDbContext<AppDbContext>()
                .ResolveWith<OrderResolvers>(c => c.GetDeliveries(default, default, default, default))
                .Type<ListType<OrderDeliveryType>>();

            descriptor.Field(c => c.PurchaseOrders)
                .Name("purchaseOrders")
                .UseDbContext<AppDbContext>()
                .ResolveWith<OrderResolvers>(c => c.GetPurchaseOrders(default, default, default, default))
                .Type<ListType<OrderDeliveryType>>();
        }

        private class OrderResolvers
        {
            public decimal GetTotalFees(Order order)
            {
                return order.FeesPrice + order.DonationFeesPrice;
            }

            public Task<User> GetUser(Order order, UsersByIdBatchDataLoader usersDataLoader,
                CancellationToken token)
            {
                if (!order.UserId.HasValue)
                    return null;

                return usersDataLoader.LoadAsync(order.UserId.Value, token);
            }

            public async Task<IEnumerable<OrderProduct>> GetProducts(Order order, [ScopedService] AppDbContext context,
                OrderProductsByIdBatchDataLoader orderProductsDataLoader, CancellationToken token)
            {
                var productsId = await context.Set<OrderProduct>()
                    .Where(o => o.OrderId == order.Id)
                    .Select(o => o.Id)
                    .ToListAsync(token);

                return await orderProductsDataLoader.LoadAsync(productsId, token);
            }

            public async Task<IEnumerable<OrderDelivery>> GetDeliveries(Order order,
                [ScopedService] AppDbContext context, OrderDeliveriesByIdBatchDataLoader orderDeliveriesDataLoader,
                CancellationToken token)
            {
                var deliveriesId = await context.Set<OrderDelivery>()
                    .Where(o => o.OrderId == order.Id)
                    .Select(o => o.Id)
                    .ToListAsync(token);

                return await orderDeliveriesDataLoader.LoadAsync(deliveriesId, token);
            }

            public async Task<IEnumerable<PurchaseOrder>> GetPurchaseOrders(Order order,
                [ScopedService] AppDbContext context, PurchaseOrdersByIdBatchDataLoader purchaseOrdersDataLoader,
                CancellationToken token)
            {
                var purchaseOrdersId = await context.PurchaseOrders
                    .Where(o => o.OrderId == order.Id)
                    .Select(o => o.Id)
                    .ToListAsync(token);

                return await purchaseOrdersDataLoader.LoadAsync(purchaseOrdersId, token);
            }
        }
    }
}