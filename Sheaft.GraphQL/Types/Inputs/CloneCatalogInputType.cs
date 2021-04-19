using HotChocolate.Types;
using Sheaft.Application.Models;
using Sheaft.Mediatr.Catalog.Commands;

namespace Sheaft.GraphQL.Types.Inputs
{
    public class CloneCatalogInputType : SheaftInputType<CloneCatalogCommand>
    {
        protected override void Configure(IInputObjectTypeDescriptor<CloneCatalogCommand> descriptor)
        {
            descriptor.Name("CloneCatalogInput");
            descriptor.Field(c => c.Name);

            descriptor.Field(c => c.CatalogId)
                .Name("Id")
                .Type<NonNullType<IdType>>();

            descriptor.Field(c => c.Percent);
        }
    }
}