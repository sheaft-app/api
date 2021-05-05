using HotChocolate.Types;
using Sheaft.Domain;
using Sheaft.Mediatr.Catalog.Commands;

namespace Sheaft.GraphQL.Types.Inputs
{
    public class DeleteCatalogsInputType : SheaftInputType<DeleteCatalogsCommand>
    {
        protected override void Configure(IInputObjectTypeDescriptor<DeleteCatalogsCommand> descriptor)
        {
            base.Configure(descriptor);

            descriptor.Name("DeleteCatalogsInput");
            
            descriptor
                .Field(c => c.CatalogIds)
                .Name("ids")
                .ID(nameof(Catalog));
        }
    }
}