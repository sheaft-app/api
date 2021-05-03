using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using HotChocolate;
using HotChocolate.Resolvers;
using HotChocolate.Types;
using Microsoft.EntityFrameworkCore;
using Sheaft.Application.Models;
using Sheaft.Domain;
using Sheaft.GraphQL.Catalogs;
using Sheaft.GraphQL.QuickOrders;
using Sheaft.GraphQL.Tags;
using Sheaft.GraphQL.Users;
using Sheaft.Infrastructure.Persistence;

namespace Sheaft.GraphQL.Types.Outputs
{
    public class QuickOrderType : SheaftOutputType<QuickOrder>
    {
        protected override void Configure(IObjectTypeDescriptor<QuickOrder> descriptor)
        {
            base.Configure(descriptor);

            descriptor
                .ImplementsNode()
                .IdField(c => c.Id)
                .ResolveNode((ctx, id) =>
                    ctx.DataLoader<QuickOrdersByIdBatchDataLoader>().LoadAsync(id, ctx.RequestAborted));
            
            descriptor
                .Field(c => c.CreatedOn)
                .Name("createdOn");
                
            descriptor
                .Field(c => c.UpdatedOn)
                .Name("updatedOn");
                
            descriptor
                .Field(c => c.IsDefault)
                .Name("isDefault");
                
            descriptor
                .Field(c => c.Description)
                .Name("description");

            descriptor
                .Field(c => c.Name)
                .Name("name")
                .Type<NonNullType<StringType>>();

            descriptor
                .Field(c => c.User)
                .Name("user")
                .ResolveWith<QuickOrderResolvers>(c => c.GetUser(default, default, default))
                .Type<NonNullType<UserType>>();

            descriptor
                .Field(c => c.Products)
                .Name("products")
                .UseDbContext<AppDbContext>()
                .ResolveWith<QuickOrderResolvers>(c => c.GetProducts(default, default, default, default));
        }

        private class QuickOrderResolvers
        {
            public Task<User> GetUser(QuickOrder quickOrder, UsersByIdBatchDataLoader usersDataLoader,
                CancellationToken token)
            {
                return usersDataLoader.LoadAsync(quickOrder.UserId, token);
            }
            
            public async Task<IEnumerable<QuickOrderProduct>> GetProducts(QuickOrder quickOrder, 
                [ScopedService] AppDbContext context, 
                QuickOrderProductsByIdBatchDataLoader quickOrderProductsDataLoader, CancellationToken token)
            {
                var productsId = await context.Set<QuickOrderProduct>()
                    .Where(op => op.QuickOrderId == quickOrder.Id)
                    .Select(op => op.Id)
                    .ToListAsync(token);

                return await quickOrderProductsDataLoader.LoadAsync(productsId, token);
            }
        }
    }
}
