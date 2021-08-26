using HotChocolate.Resolvers;
using HotChocolate.Types;
using Sheaft.Domain;
using Sheaft.GraphQL.Nationalities;

namespace Sheaft.GraphQL.Types.Outputs
{
    public class NationalityType : SheaftOutputType<Nationality>
    {
        protected override void Configure(IObjectTypeDescriptor<Nationality> descriptor)
        {
            base.Configure(descriptor);

            descriptor
                .ImplementsNode()
                .IdField(c => c.Id)
                .ResolveNode((ctx, id) =>
                    ctx.DataLoader<NationalitiesByIdBatchDataLoader>().LoadAsync(id, ctx.RequestAborted));

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
