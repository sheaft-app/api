using HotChocolate.Types;
using Sheaft.Domain;
using Sheaft.Mediatr.Catalog.Commands;

namespace Sheaft.GraphQL.Types.Inputs
{
    public class SetCatalogsAvailabilityInputType : SheaftInputType<SetCatalogsAvailabilityCommand>
    {
        protected override void Configure(IInputObjectTypeDescriptor<SetCatalogsAvailabilityCommand> descriptor)
        {
            base.Configure(descriptor);

            descriptor.Name("SetCatalogsAvailabilityInput");
            
            descriptor
                .Field(c => c.Available)
                .Name("available");

            descriptor
                .Field(c => c.CatalogIds)
                .Name("ids")
                .ID(nameof(Catalog));
        }
    }
}