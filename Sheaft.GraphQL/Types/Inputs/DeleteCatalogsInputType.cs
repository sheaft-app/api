using HotChocolate.Types;
using Sheaft.Mediatr.Catalog.Commands;

namespace Sheaft.GraphQL.Types.Inputs
{
    public class DeleteCatalogsInputType : SheaftInputType<DeleteCatalogsCommand>
    {
        protected override void Configure(IInputObjectTypeDescriptor<DeleteCatalogsCommand> descriptor)
        {
            descriptor.Name("DeleteCatalogsInput");
            descriptor.Field(c => c.CatalogIds)
                .Name("ids")
                .Type<NonNullType<ListType<IdType>>>();
        }
    }
}