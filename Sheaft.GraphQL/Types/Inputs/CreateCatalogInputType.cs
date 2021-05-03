using HotChocolate.Types;
using Sheaft.Mediatr.Catalog.Commands;

namespace Sheaft.GraphQL.Types.Inputs
{
    public class CreateCatalogInputType : SheaftInputType<CreateCatalogCommand>
    {
        protected override void Configure(IInputObjectTypeDescriptor<CreateCatalogCommand> descriptor)
        {
            descriptor.Name("CreateCatalogInput");
            descriptor.Field(c => c.Name);

            descriptor.Field(c => c.IsAvailable);
            descriptor.Field(c => c.IsDefault);
        }
    }
}