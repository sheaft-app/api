using HotChocolate.Resolvers;
using HotChocolate.Types;
using Sheaft.Domain;
using Sheaft.GraphQL.BankAccounts;
using Sheaft.GraphQL.Catalogs;

namespace Sheaft.GraphQL.Types.Outputs
{
    public class BankAccountType : SheaftOutputType<BankAccount>
    {
        protected override void Configure(IObjectTypeDescriptor<BankAccount> descriptor)
        {
            base.Configure(descriptor);

            descriptor
                .ImplementsNode()
                .IdField(c => c.Id)
                .ResolveNode((ctx, id) =>
                    ctx.DataLoader<BankAccountsByIdBatchDataLoader>().LoadAsync(id, ctx.RequestAborted));
            
            descriptor
                .Field(c => c.IBAN)
                .Name("iban");
                
            descriptor
                .Field(c => c.BIC)
                .Name("bic");
                
            descriptor
                .Field(c => c.Owner)
                .Name("owner");
                
            descriptor
                .Field(c => c.Line1)
                .Name("line1");
                
            descriptor
                .Field(c => c.Line2)
                .Name("line2");
                
            descriptor
                .Field(c => c.Zipcode)
                .Name("zipcode");
                
            descriptor
                .Field(c => c.City)
                .Name("city");
                
            descriptor
                .Field(c => c.Country)
                .Name("country");
                
        }
    }
}