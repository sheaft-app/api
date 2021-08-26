using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using HotChocolate;
using HotChocolate.Resolvers;
using HotChocolate.Types;
using Microsoft.EntityFrameworkCore;
using Sheaft.Domain;
using Sheaft.GraphQL.Donations;
using Sheaft.GraphQL.Users;
using Sheaft.Infrastructure.Persistence;

namespace Sheaft.GraphQL.Types.Outputs
{
    public class DonationType : SheaftOutputType<Donation>
    {
        protected override void Configure(IObjectTypeDescriptor<Donation> descriptor)
        {
            base.Configure(descriptor);

            descriptor
                .ImplementsNode()
                .IdField(c => c.Id)
                .ResolveNode((ctx, id) =>
                    ctx.DataLoader<DonationsByIdBatchDataLoader>().LoadAsync(id, ctx.RequestAborted));

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
                .UseDbContext<QueryDbContext>()
                .ResolveWith<DonationResolvers>(c => c.GetUser(default, default, default, default));
        }

        private class DonationResolvers
        {
            public async Task<User> GetUser(Donation donation, [ScopedService] QueryDbContext context,
                UsersByIdBatchDataLoader usersDataLoader, CancellationToken token)
            {
                var userId = await context.Wallets
                    .Where(u => u.Id == donation.CreditedWalletId)
                    .Select(u => u.UserId)
                    .SingleOrDefaultAsync(token);

                if (userId == Guid.Empty)
                    return null;

                return await usersDataLoader.LoadAsync(userId, token);
            }
        }
    }
}