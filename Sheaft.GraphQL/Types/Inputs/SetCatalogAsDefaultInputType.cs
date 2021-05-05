using HotChocolate.Types;
using Sheaft.Domain;
using Sheaft.Mediatr.Catalog.Commands;

namespace Sheaft.GraphQL.Types.Inputs
{
    public class SetCatalogAsDefaultInputType : SheaftInputType<SetCatalogAsDefaultCommand>
    {
        protected override void Configure(IInputObjectTypeDescriptor<SetCatalogAsDefaultCommand> descriptor)
        {
            base.Configure(descriptor);

            descriptor.Name("SetCatalogAsDefaultInput");
            
            descriptor
                .Field(c => c.CatalogId)
                .Name("id")
                .ID(nameof(Catalog));
        }
    }
}