using HotChocolate.Resolvers;
using HotChocolate.Types;
using Sheaft.Domain;
using Sheaft.GraphQL.Countries;

namespace Sheaft.GraphQL.Types.Outputs
{
    public class CountryType : SheaftOutputType<Country>
    {
        protected override void Configure(IObjectTypeDescriptor<Country> descriptor)
        {
            base.Configure(descriptor);

            descriptor
                .ImplementsNode()
                .IdField(c => c.Id)
                .ResolveNode((ctx, id) =>
                    ctx.DataLoader<CountriesByIdBatchDataLoader>().LoadAsync(id, ctx.RequestAborted));
            
            descriptor
                .Field(c => c.Name)
                .Name("name")
                .Type<NonNullType<StringType>>();

            descriptor
                .Field(c => c.Alpha2)
                .Name("code")
                .Type<NonNullType<StringType>>();
        }
    }
}
