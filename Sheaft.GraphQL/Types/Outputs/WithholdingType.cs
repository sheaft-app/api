using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using HotChocolate;
using HotChocolate.Resolvers;
using HotChocolate.Types;
using Microsoft.EntityFrameworkCore;
using Sheaft.Domain;
using Sheaft.GraphQL.Catalogs;
using Sheaft.GraphQL.Tags;
using Sheaft.GraphQL.Users;
using Sheaft.GraphQL.Withholdings;
using Sheaft.Infrastructure.Persistence;

namespace Sheaft.GraphQL.Types.Outputs
{
    public class WithholdingType : SheaftOutputType<Withholding>
    {
        protected override void Configure(IObjectTypeDescriptor<Withholding> descriptor)
        {
            base.Configure(descriptor);

            descriptor
                .ImplementsNode()
                .IdField(c => c.Id)
                .ResolveNode((ctx, id) =>
                    ctx.DataLoader<WithholdingsByIdBatchDataLoader>().LoadAsync(id, ctx.RequestAborted));
            
            descriptor
                .Field(c => c.Status)
                .Name("status");
                
            descriptor
                .Field(c => c.Fees)
                .Name("fees");
                
            descriptor
                .Field(c => c.Debited)
                .Name("debited");
                
            descriptor
                .Field(c => c.Reference)
                .Name("reference");
                
            descriptor
                .Field(c => c.CreatedOn)
                .Name("createdOn");
                
            descriptor
                .Field(c => c.UpdatedOn)
                .Name("updatedOn");
                
            descriptor
                .Field(c => c.ExecutedOn)
                .Name("executedOn");
                
            descriptor
                .Field(c => c.Kind)
                .Name("kind");
                
            descriptor
                .Field(c => c.Status)
                .Name("status");

            descriptor
                .Field("creditedUser")
                .UseDbContext<AppDbContext>()
                .ResolveWith<WithholdingResolvers>(c => c.GetUser(default, default, default, default))
                .Type<NonNullType<UserType>>();
        }

        private class WithholdingResolvers
        {
            public async Task<User> GetUser(Withholding withholding, [ScopedService] AppDbContext context,
                UsersByIdBatchDataLoader usersDataLoader, CancellationToken token)
            {
                var userId = await context.Wallets
                    .Where(w => w.Id == withholding.CreditedWalletId)
                    .Select(w => w.UserId)
                    .SingleAsync(token);

                return await usersDataLoader.LoadAsync(userId, token);
            }
        }
    }
}