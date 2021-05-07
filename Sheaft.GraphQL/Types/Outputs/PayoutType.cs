using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using HotChocolate;
using HotChocolate.Resolvers;
using HotChocolate.Types;
using Microsoft.EntityFrameworkCore;
using Sheaft.Domain;
using Sheaft.GraphQL.BankAccounts;
using Sheaft.GraphQL.Catalogs;
using Sheaft.GraphQL.Payouts;
using Sheaft.GraphQL.Tags;
using Sheaft.GraphQL.Users;
using Sheaft.Infrastructure.Persistence;

namespace Sheaft.GraphQL.Types.Outputs
{
    public class PayoutType : SheaftOutputType<Payout>
    {
        protected override void Configure(IObjectTypeDescriptor<Payout> descriptor)
        {
            base.Configure(descriptor);

            descriptor
                .ImplementsNode()
                .IdField(c => c.Id)
                .ResolveNode((ctx, id) => 
                    ctx.DataLoader<PayoutsByIdBatchDataLoader>().LoadAsync(id, ctx.RequestAborted));
            
            descriptor
                .Field(c => c.Status)
                .Name("status");
                
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
                .Field("debitedUser")
                .UseDbContext<QueryDbContext>()
                .ResolveWith<PayoutResolvers>(c => c.GetUser(default, default, default, default))
                .Type<NonNullType<UserType>>();
                
            descriptor
                .Field(c => c.BankAccount)
                .Name("bankAccount")
                .ResolveWith<PayoutResolvers>(c => c.GetBankAccount(default, default, default))
                .Type<NonNullType<BankAccountType>>();
        }

        private class PayoutResolvers
        {
            public async Task<User> GetUser(Payout payout, [ScopedService] QueryDbContext context, UsersByIdBatchDataLoader usersDataLoader, CancellationToken token)
            {
                var userId = await context.Wallets
                    .Where(w => w.Id == payout.DebitedWalletId)
                    .Select(w => w.UserId)
                    .SingleAsync(token);

                return await usersDataLoader.LoadAsync(userId, token);
            }
            
            public Task<BankAccount> GetBankAccount(Payout payout, BankAccountsByIdBatchDataLoader bankAccountsDataLoader, CancellationToken token)
            {
                return bankAccountsDataLoader.LoadAsync(payout.BankAccountId, token);
            }
        }
    }
}
