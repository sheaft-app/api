using HotChocolate.Types;
using Sheaft.Application.Models;
using Sheaft.Mediatr.Catalog.Commands;

namespace Sheaft.GraphQL.Types.Inputs
{
    public class UpdateCatalogInputType : SheaftInputType<UpdateCatalogCommand>
    {
        protected override void Configure(IInputObjectTypeDescriptor<UpdateCatalogCommand> descriptor)
        {
            descriptor.Name("UpdateCatalogInput");
            descriptor.Field(c => c.Name);

            descriptor.Field(c => c.CatalogId)
                .Name("id")
                .Type<NonNullType<IdType>>();

            descriptor.Field(c => c.IsAvailable);
            descriptor.Field(c => c.IsDefault);
        }
    }
}