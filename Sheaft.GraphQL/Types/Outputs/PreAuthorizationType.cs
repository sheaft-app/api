using System.Threading;
using System.Threading.Tasks;
using HotChocolate;
using HotChocolate.Resolvers;
using HotChocolate.Types;
using Sheaft.Application.Models;
using Sheaft.Domain;
using Sheaft.GraphQL.Catalogs;
using Sheaft.GraphQL.Orders;
using Sheaft.GraphQL.PreAuthorizations;
using Sheaft.Infrastructure.Persistence;

namespace Sheaft.GraphQL.Types.Outputs
{
    public class PreAuthorizationType : SheaftOutputType<PreAuthorization>
    {
        protected override void Configure(IObjectTypeDescriptor<PreAuthorization> descriptor)
        {
            base.Configure(descriptor);

            descriptor
                .ImplementsNode()
                .IdField(c => c.Id)
                .ResolveNode((ctx, id) =>
                    ctx.DataLoader<PreAuthorizationsByIdBatchDataLoader>().LoadAsync(id, ctx.RequestAborted));

            descriptor
                .Field(c => c.Status)
                .Name("status");

            descriptor
                .Field(c => c.PaymentStatus)
                .Name("paymentStatus");

            descriptor
                .Field(c => c.SecureModeRedirectUrl)
                .Name("secureModeRedirectURL");
            
            descriptor
                .Field(c => c.SecureModeNeeded)
                .Name("secureModeNeeded");

            descriptor
                .Field(c => c.Order)
                .Name("order")
                .ResolveWith<PreAuthorizationResolvers>(c => c.GetOrder(default, default, default))
                .Type<NonNullType<OrderType>>();
        }

        private class PreAuthorizationResolvers
        {
            public Task<Order> GetOrder(PreAuthorization preAuthorization,
                OrdersByIdBatchDataLoader ordersDataLoader, CancellationToken token)
            {
                return ordersDataLoader.LoadAsync(preAuthorization.OrderId, token);
            }
        }
    }
}