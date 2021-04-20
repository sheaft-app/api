using HotChocolate.Types;
using Sheaft.Mediatr.Catalog.Commands;

namespace Sheaft.GraphQL.Types.Inputs
{
    public class SetCatalogsAvailabilityInputType : SheaftInputType<SetCatalogsAvailabilityCommand>
    {
        protected override void Configure(IInputObjectTypeDescriptor<SetCatalogsAvailabilityCommand> descriptor)
        {
            descriptor.Name("SetCatalogsAvailabilityInput");
            descriptor.Field(c => c.Available);

            descriptor.Field(c => c.CatalogIds)
                .Name("ids")
                .Type<NonNullType<ListType<IdType>>>();
        }
    }
}